using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Audit;

public class AuditDTOModelFileGenerator : AuditDTOModelFileGenerator<IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase>>
{
    public AuditDTOModelFileGenerator(IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

public class AuditDTOModelFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase>
{
    public AuditDTOModelFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }

    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new AuditFileFactory(this.Configuration, this.GetCodeNamespace());

    }

    private CodeNamespace GetCodeNamespace()
    {
        return new CodeNamespace(this.Configuration.Namespace)
               {
                       Types =
                       {
                               this.GetRootPropertyRevisionDTO(),
                               this.GetPropertyRevisionDTO(),
                               this.GetPropertyRevisionDTOBase(),
                       },
               };
    }

    private CodeTypeDeclaration GetRootPropertyRevisionDTO()
    {
        var baseType = new CodeTypeReference(typeof(DomainObjectPropertiesRevisionDTO<,>))
                       .Self(z => z.TypeArguments.Add(typeof(Guid)))
                       .Self(z => z.TypeArguments.Add(this.Configuration.PropertyRevisionTypeName));

        return new CodeTypeDeclaration(this.Configuration.DomainObjectPropertiesRevisionDTOTypeName)
               {
                       CustomAttributes = new CodeAttributeDeclarationCollection(new[] { new CodeAttributeDeclaration(typeof(DataContractAttribute).FullName) }),
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
               .Self(z => z.Members.AddRange(GetPropertyRevisionsCodeConstructor().ToArray()));
    }

    private IEnumerable<CodeAttributeDeclaration> GetCustomerAttributeNames()
    {
        yield return new CodeAttributeDeclaration(typeof(DataContractAttribute).FullName);

        var allDomainPersistentObject = this.Configuration.DomainTypes
                                            .Where(z => !z.GetCustomAttributes<NotAuditedClassAttribute>().Any())
                                            .Where(z => !z.IsAbstract)
                                            .Where(z => this.Configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(z))
                                            .OrderBy(z => z.Name)
                                            .ToList();

        var allPropertyInfoCollection = allDomainPersistentObject
                                        .SelectMany(z => z.GetProperties())
                                        .Where(z => z.GetCustomAttributes<CustomSerializationAttribute>().EmptyIfNull().All(q => q.Mode != CustomSerializationMode.Ignore))
                                        .Select(z => z.PropertyType)
                                        .Where(z => !z.IsProjection())
                                        .Where(z => !z.GetCustomAttributes<NotAuditedClassAttribute>().Any())
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

    private IEnumerable<CodeTypeReference> GetMaybePropertyCodeTypeReferences(CodeTypeReference typeReference)
    {
        return Maybe<object>
               .GetKnownTypes()
               .Select(z => z.GetGenericTypeDefinition())
               .Select(z => new CodeTypeReference(z.GetGenericTypeDefinition()).Self(q => q.TypeArguments.Add(typeReference)));
    }
    private IEnumerable<CodeTypeReference> GetPropertyCodeTypeRefences(Type propertyType)
    {
        if (this.Configuration.IsDomainObject(propertyType))
        {
            if (this.Configuration.Environment.ServerDTO.GeneratePolicy.Used(propertyType, DTOGenerator.FileType.SimpleDTO))
            {
                var result = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(propertyType, DTOGenerator.FileType.SimpleDTO);

                yield return result;

                foreach (var maybePropertyCodeTypeReference in this.GetMaybePropertyCodeTypeReferences(result))
                {
                    yield return maybePropertyCodeTypeReference;
                }
            }
        }
        else if (propertyType.IsCollection()
                 && this.Configuration.IsDomainObject(propertyType.GetCollectionOrArrayElementType()))
        {
            if (this.Configuration.Environment.ServerDTO.GeneratePolicy.Used(propertyType.GetCollectionOrArrayElementType(), DTOGenerator.FileType.SimpleDTO))
            {
                var type = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(propertyType.GetCollectionOrArrayElementType(), DTOGenerator.FileType.SimpleDTO);
                var result = new CodeTypeReference(propertyType.GetGenericTypeDefinition()).Self(z => z.TypeArguments.Add(type));

                yield return result;

                foreach (var maybePropertyCodeTypeReference in this.GetMaybePropertyCodeTypeReferences(result))
                {
                    yield return maybePropertyCodeTypeReference;
                }
            }
        }
        else
        {
            var result = new CodeTypeReference(propertyType);

            yield return result;

            foreach (var maybePropertyCodeTypeReference in this.GetMaybePropertyCodeTypeReferences(result))
            {
                yield return maybePropertyCodeTypeReference;
            }
        }
    }

    private CodeTypeDeclaration GetPropertyRevisionDTO()
    {
        var field = new CodeMemberField("TValue", "Value")
                    .Self(q => q.Attributes = MemberAttributes.Public)
                    .Self(q => q.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(DataMemberAttribute).FullName)));
        return new CodeTypeDeclaration(this.Configuration.PropertyRevisionTypeName)
               {
                       CustomAttributes = new CodeAttributeDeclarationCollection(new[]
                                                                                 {
                                                                                         new CodeAttributeDeclaration(typeof (DataContractAttribute).FullName),
                                                                                 }),
               }
               .Self(z => z.BaseTypes.Add(this.Configuration.PropertyRevisionTypeName))
               .Self(z => z.TypeParameters.Add("TValue"))
               .Self(z => z.Members.AddRange(GetPropertyRevisionsCodeConstructor().ToArray()))
               .Self(z => z.Members.Add(field));
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

    private class AuditFileFactory : GeneratorConfigurationContainer<TConfiguration>, ICodeFile
    {
        private readonly CodeNamespace _codeNamespace;

        public AuditFileFactory(TConfiguration configuration, CodeNamespace codeNamespace)
                : base(configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (codeNamespace == null) throw new ArgumentNullException(nameof(codeNamespace));

            this._codeNamespace = codeNamespace;
        }

        public CodeNamespace GetRenderData()
        {
            return this._codeNamespace;
        }

        public string Filename => this.Configuration.PropertyRevisionTypeName;
    }
}
