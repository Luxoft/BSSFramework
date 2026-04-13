using System.CodeDom;
using System.Runtime.Serialization;

using CommonFramework;

using Framework.BLL.Domain.Serialization;
using Framework.BLL.DTOMapping.Domain;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.Core;
using Framework.Database.Domain;
using Framework.Database.Mapping;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.DTOGenerator.Audit;

public class AuditDTOModelFileGenerator(IAuditDTOGeneratorConfiguration<IAuditDTOGenerationEnvironment> configuration)
    : AuditDTOModelFileGenerator<IAuditDTOGeneratorConfiguration<IAuditDTOGenerationEnvironment>>(configuration);

public class AuditDTOModelFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IAuditDTOGeneratorConfiguration<IAuditDTOGenerationEnvironment>
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new AuditFileFactory(this.Configuration, this.GetCodeNamespace());

    }

    private CodeNamespace GetCodeNamespace() =>
        new(this.Configuration.Namespace)
        {
            Types = { this.GetRootPropertyRevisionDTO(), this.GetPropertyRevisionDTO(), this.GetPropertyRevisionDTOBase(), },
        };

    private CodeTypeDeclaration GetRootPropertyRevisionDTO()
    {
        var baseType = new CodeTypeReference(typeof(DomainObjectPropertiesRevisionDTO<,>))
                       .Self(z => z.TypeArguments.Add(typeof(Guid)))
                       .Self(z => z.TypeArguments.Add(this.Configuration.PropertyRevisionTypeName));

        return new CodeTypeDeclaration(this.Configuration.DomainObjectPropertiesRevisionDTOTypeName)
            {
                CustomAttributes = new CodeAttributeDeclarationCollection([new CodeAttributeDeclaration(typeof(DataContractAttribute).FullName)]),
            }
            .Self(z => z.BaseTypes.Add(baseType));
    }

    private CodeTypeDeclaration GetPropertyRevisionDTOBase()
    {
        var baseType = new CodeTypeReference(typeof(PropertyRevisionDTOBase));
        return new CodeTypeDeclaration(this.Configuration.PropertyRevisionTypeName)
               {
                   CustomAttributes = new CodeAttributeDeclarationCollection(this.GetCustomerAttributeNames().ToArray()),
               }
               .Self(z => z.BaseTypes.Add(baseType))
               .Self(z => z.Members.AddRange([.. GetPropertyRevisionsCodeConstructor()]));
    }

    private IEnumerable<CodeAttributeDeclaration> GetCustomerAttributeNames()
    {
        yield return new CodeAttributeDeclaration(typeof(DataContractAttribute).FullName);

        var allDomainPersistentObject = this.Configuration.DomainTypes
                                            .Where(z => !this.Configuration.Environment.MetadataProxyProvider.Wrap(z).HasAttribute<NotAuditedClassAttribute>())
                                            .Where(z => !z.IsAbstract)
                                            .Where(z => this.Configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(z))
                                            .OrderBy(z => z.Name)
                                            .ToList();

        var allPropertyInfoCollection = allDomainPersistentObject
                                        .SelectMany(z => z.GetProperties())
                                        .Where(z => this.Configuration.Environment.MetadataProxyProvider.Wrap(z).GetCustomAttributes<CustomSerializationAttribute>().EmptyIfNull().All(q => q.Mode != CustomSerializationMode.Ignore))
                                        .Select(z => z.PropertyType)
                                        .Where(z => !z.IsProjection())
                                        .Where(z => !this.Configuration.Environment.MetadataProxyProvider.Wrap(z).HasAttribute<NotAuditedClassAttribute>())
                                        .Where(z => !z.IsAbstract)
                                        .Distinct()
                                        .OrderBy(z => z.Name)
                                        .ToList();

        foreach (var propertyInfo in allPropertyInfoCollection)
        {
            foreach (var typeReference in this.GetPropertyCodeTypeRefences(propertyInfo))
            {
                var dtoPropertyChangedType = new CodeTypeReference(this.Configuration.PropertyRevisionTypeName);
                dtoPropertyChangedType.TypeArguments.Add(typeReference);

                var typeOfCodeExpresson = dtoPropertyChangedType.ToTypeOfExpression();
                var result = new CodeAttributeDeclaration(typeof(KnownTypeAttribute).FullName);
                result.Self(z => z.Arguments.Add(new CodeAttributeArgument(typeOfCodeExpresson)));

                yield return result;
            }
        }
    }

    private IEnumerable<CodeTypeReference> GetPropertyCodeTypeRefences(Type propertyType)
    {
        if (this.Configuration.IsDomainObject(propertyType))
        {
            if (this.Configuration.Environment.ServerDTO.GeneratePolicy.Used(propertyType, BaseFileType.SimpleDTO))
            {
                var result = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(propertyType, BaseFileType.SimpleDTO);

                yield return result;
            }
        }
        else if (propertyType.IsCollection()
                 && this.Configuration.IsDomainObject(propertyType.GetCollectionOrArrayElementType()!))
        {
            if (this.Configuration.Environment.ServerDTO.GeneratePolicy.Used(propertyType.GetCollectionOrArrayElementType()!, BaseFileType.SimpleDTO))
            {
                var type = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(propertyType.GetCollectionOrArrayElementType(), BaseFileType.SimpleDTO);
                var result = new CodeTypeReference(propertyType.GetGenericTypeDefinition()).Self(z => z.TypeArguments.Add(type));

                yield return result;
            }
        }
        else
        {
            var result = new CodeTypeReference(propertyType);

            yield return result;
        }
    }

    private CodeTypeDeclaration GetPropertyRevisionDTO()
    {
        var field = new CodeMemberField("TValue", "value") { Attributes = MemberAttributes.Private, };

        var fieldExpr = new CodeThisReferenceExpression().ToFieldReference(field);

        var property = new CodeMemberProperty
                       {
                           Name = "Value",
                           Type = new CodeTypeReference("TValue"),
                           Attributes = MemberAttributes.Public,
                           CustomAttributes = { new CodeAttributeDeclaration(typeof(DataMemberAttribute).FullName) },
                           GetStatements = { fieldExpr.ToMethodReturnStatement() },
                           SetStatements = { new CodePropertySetValueReferenceExpression().ToAssignStatement(fieldExpr) }
                       };

        return new CodeTypeDeclaration(this.Configuration.PropertyRevisionTypeName)
               {
                   CustomAttributes = new CodeAttributeDeclarationCollection([new CodeAttributeDeclaration(typeof(DataContractAttribute).FullName)]),
               }
               .Self(z => z.BaseTypes.Add(this.Configuration.PropertyRevisionTypeName))
               .Self(z => z.TypeParameters.Add("TValue"))
               .Self(z => z.Members.AddRange([.. GetPropertyRevisionsCodeConstructor()]))
               .Self(z => z.Members.Add(field))
               .Self(z => z.Members.Add(property));
    }

    private static IEnumerable<CodeConstructor> GetPropertyRevisionsCodeConstructor()
    {
        yield return
            new CodeConstructor()
                .Self(z => z.Attributes = MemberAttributes.Public)
                .Self(z => z.Parameters.Add(new CodeParameterDeclarationExpression(typeof(RevisionInfoBase), "info")))
                .Self(q => q.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("info")));
        yield return new CodeConstructor().Self(z => z.Attributes = MemberAttributes.Public);
    }

    private class AuditFileFactory(TConfiguration configuration, CodeNamespace codeNamespace) : GeneratorConfigurationContainer<TConfiguration>(configuration), ICodeFile
    {
        public CodeNamespace GetRenderData() => codeNamespace;

        public string Filename => this.Configuration.PropertyRevisionTypeName;
    }
}
