using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class ServerPrimitiveDTOMappingServiceBaseFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IServerMappingServiceExternalMethodGenerator> _externalGenerators;


    public ServerPrimitiveDTOMappingServiceBaseFileFactory(TConfiguration configuration, IEnumerable<IServerMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this._externalGenerators = externalGenerators.ToReadOnlyCollection();

        this.BaseReference = typeof(DTOMappingService<,,,,>).ToTypeReference(

                                                                             this.Configuration.BLLContextTypeReference,
                                                                             this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                                                             this.Configuration.Environment.AuditPersistentDomainObjectBaseType.ToTypeReference(),
                                                                             this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                                                                             this.Configuration.VersionType.ToTypeReference());
    }


    public override FileType FileType { get; } = ServerFileType.ServerPrimitiveDTOMappingServiceBase;


    public override CodeTypeReference BaseReference { get; }


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        yield return this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Abstract,
                       TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }



        var contextParameter = this.Configuration.BLLContextTypeReference.ToParameterDeclarationExpression("context");

        yield return new CodeConstructor
                     {
                             Attributes = MemberAttributes.Family,
                             Parameters = { contextParameter },
                             BaseConstructorArgs = { contextParameter.ToVariableReferenceExpression() }
                     };



        foreach (var fieldFileFactory in this._externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetServerMappingServiceMethods())
            {
                yield return method;
            }
        }

        yield return this.GetToPersistentDomainObjectMethod();
        yield return this.GetToPersistentDomainObjectByFactoryMethod();
        yield return this.GetToDomainObjectBaseMethod();
        yield return this.GetMapToDomainObjectMethod();
    }


    private CodeMemberMethod GetToPersistentDomainObjectMethod()
    {
        var mappingServiceTypeRef = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

        var domainObjectGenericParameter = new CodeTypeParameter
                                           {
                                                   Name = "TDomainObject",
                                                   Constraints = { this.Configuration.Environment.PersistentDomainObjectBaseType }
                                           };

        var mappingObjectGenericParameter = new CodeTypeParameter
                                            {
                                                    Name = "TMappingObject",
                                                    Constraints =
                                                    {
                                                            typeof(IMappingObject<,,>).ToTypeReference(mappingServiceTypeRef, domainObjectGenericParameter.ToTypeReference(), this.Configuration.Environment.GetIdentityType().ToTypeReference())
                                                    }
                                            };

        var mappingObjectMethodParameter = mappingObjectGenericParameter.ToTypeReference().ToParameterDeclarationExpression("mappingObject");

        var domainObjectVariableStatement = new CodeVariableDeclarationStatement(domainObjectGenericParameter.ToTypeReference(), "domainObject",

                                                                                 new CodeThisReferenceExpression().ToMethodReferenceExpression("GetById", domainObjectGenericParameter.ToTypeReference())
                                                                                         .ToMethodInvokeExpression(mappingObjectMethodParameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty), IdCheckMode.CheckAll.ToPrimitiveExpression()));

        return new CodeMemberMethod
               {
                       Name = this.Configuration.ToDomainObjectMethodName,
                       Attributes = MemberAttributes.Family,
                       TypeParameters = { mappingObjectGenericParameter, domainObjectGenericParameter },
                       Parameters = { mappingObjectMethodParameter },
                       Statements =
                       {
                               domainObjectVariableStatement,
                               new CodeThisReferenceExpression().ToMethodInvokeExpression(this.Configuration.MapToDomainObjectMethodName, mappingObjectMethodParameter.ToVariableReferenceExpression(), domainObjectVariableStatement.ToVariableReferenceExpression()),
                               domainObjectVariableStatement.ToVariableReferenceExpression().ToMethodReturnStatement()
                       },
                       ReturnType = domainObjectGenericParameter.ToTypeReference()
               };
    }

    private CodeMemberMethod GetToPersistentDomainObjectByFactoryMethod()
    {
        var mappingServiceTypeRef = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

        var domainObjectGenericParameter = new CodeTypeParameter
                                           {
                                                   Name = "TDomainObject",
                                                   Constraints = { this.Configuration.Environment.PersistentDomainObjectBaseType }
                                           };

        var mappingObjectGenericParameter = new CodeTypeParameter
                                            {
                                                    Name = "TMappingObject",
                                                    Constraints =
                                                    {
                                                            typeof(IMappingObject<,,>).ToTypeReference(mappingServiceTypeRef, domainObjectGenericParameter.ToTypeReference(), this.Configuration.Environment.GetIdentityType().ToTypeReference())
                                                    }
                                            };

        var mappingObjectMethodParameter = mappingObjectGenericParameter.ToTypeReference().ToParameterDeclarationExpression("mappingObject");
        var createFuncParameter = typeof(Func<>).ToTypeReference(domainObjectGenericParameter.ToTypeReference()).ToParameterDeclarationExpression("createFunc");

        var domainObjectVariableStatement = new CodeVariableDeclarationStatement(domainObjectGenericParameter.ToTypeReference(), "domainObject",

                                                                                 new CodeThisReferenceExpression().ToMethodReferenceExpression("GetByIdOrCreate", domainObjectGenericParameter.ToTypeReference())
                                                                                         .ToMethodInvokeExpression(mappingObjectMethodParameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty), createFuncParameter.ToVariableReferenceExpression()));

        return new CodeMemberMethod
               {
                       Name = this.Configuration.ToDomainObjectMethodName,
                       Attributes = MemberAttributes.Family,
                       TypeParameters = { mappingObjectGenericParameter, domainObjectGenericParameter },
                       Parameters = { mappingObjectMethodParameter, createFuncParameter },
                       Statements =
                       {
                               domainObjectVariableStatement,
                               new CodeThisReferenceExpression().ToMethodInvokeExpression(this.Configuration.MapToDomainObjectMethodName, mappingObjectMethodParameter.ToVariableReferenceExpression(), domainObjectVariableStatement.ToVariableReferenceExpression()),
                               domainObjectVariableStatement.ToVariableReferenceExpression().ToMethodReturnStatement()
                       },
                       ReturnType = domainObjectGenericParameter.ToTypeReference()
               };
    }

    private CodeMemberMethod GetToDomainObjectBaseMethod()
    {
        var mappingServiceTypeRef = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

        var domainObjectGenericParameter = new CodeTypeParameter
                                           {
                                                   Name = "TDomainObject",
                                                   Constraints = { this.Configuration.Environment.DomainObjectBaseType },
                                                   HasConstructorConstraint = true
                                           };

        var mappingObjectGenericParameter = new CodeTypeParameter
                                            {
                                                    Name = "TMappingObject",
                                                    Constraints =
                                                    {
                                                            typeof(IMappingObject<,>).ToTypeReference(mappingServiceTypeRef, domainObjectGenericParameter.ToTypeReference())
                                                    }
                                            };

        var mappingObjectMethodParameter = mappingObjectGenericParameter.ToTypeReference().ToParameterDeclarationExpression("mappingObject");

        var domainObjectVariableStatement = new CodeVariableDeclarationStatement(domainObjectGenericParameter.ToTypeReference(), "domainObject",

                                                                                 domainObjectGenericParameter.ToTypeReference().ToObjectCreateExpression());

        return new CodeMemberMethod
               {
                       Name = this.Configuration.ToDomainObjectMethodName + "Base",
                       Attributes = MemberAttributes.Family,
                       TypeParameters = { mappingObjectGenericParameter, domainObjectGenericParameter },
                       Parameters = { mappingObjectMethodParameter },
                       Statements =
                       {
                               domainObjectVariableStatement,
                               new CodeThisReferenceExpression().ToMethodInvokeExpression(this.Configuration.MapToDomainObjectMethodName, mappingObjectMethodParameter.ToVariableReferenceExpression(), domainObjectVariableStatement.ToVariableReferenceExpression()),
                               domainObjectVariableStatement.ToVariableReferenceExpression().ToMethodReturnStatement()
                       },
                       ReturnType = domainObjectGenericParameter.ToTypeReference()
               };
    }

    private CodeMemberMethod GetMapToDomainObjectMethod()
    {
        var mappingServiceTypeRef = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerDTOMappingServiceInterface);

        var domainObjectGenericParameter = new CodeTypeParameter
                                           {
                                                   Name = "TDomainObject",
                                                   Constraints = { this.Configuration.Environment.DomainObjectBaseType }
                                           };

        var mappingObjectGenericParameter = new CodeTypeParameter
                                            {
                                                    Name = "TMappingObject",
                                                    Constraints =
                                                    {
                                                            typeof(IMappingObject<,>).ToTypeReference(mappingServiceTypeRef, domainObjectGenericParameter.ToTypeReference())
                                                    }
                                            };

        var mappingObjectMethodParameter = mappingObjectGenericParameter.ToTypeReference().ToParameterDeclarationExpression("mappingObject");

        var domainObjectMethodParameter = domainObjectGenericParameter.ToTypeReference().ToParameterDeclarationExpression("domainObject");

        return new CodeMemberMethod
               {
                       Name = this.Configuration.MapToDomainObjectMethodName,
                       Attributes = MemberAttributes.Family,
                       TypeParameters = { mappingObjectGenericParameter, domainObjectGenericParameter },
                       Parameters = { mappingObjectMethodParameter, domainObjectMethodParameter },
                       Statements =
                       {
                               mappingObjectMethodParameter.ToVariableReferenceExpression()
                                                           .ToMethodInvokeExpression(this.Configuration.MapToDomainObjectMethodName, new CodeThisReferenceExpression(), domainObjectMethodParameter.ToVariableReferenceExpression()),
                       },
                       ReturnType = typeof(void).ToTypeReference()
               };
    }
}
