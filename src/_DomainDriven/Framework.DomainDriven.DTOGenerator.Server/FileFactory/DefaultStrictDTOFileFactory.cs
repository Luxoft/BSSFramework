using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultStrictDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>, IClientMappingServiceExternalMethodGenerator
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultStrictDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new StrictCodeTypeReferenceService<TConfiguration>(this.Configuration);
    }



    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

    public override DTOFileType FileType { get; } = DTOGenerator.FileType.StrictDTO;


    protected override bool HasMapToDomainObjectMethod => this.Configuration.MapToDomainRole.HasFlag(ClientDTORole.Strict);

    protected override IPropertyAssigner MapMappingObjectToDomainObjectPropertyAssigner
    {
        get
        {
            if (this.HasMapToDomainObjectMethod)
            {
                return this.Configuration.PropertyAssignerConfigurator.GetStrictSecurityToDomainObjectPropertyAssigner(new DTOToDomainObjectPropertyAssigner<TConfiguration>(this));
            }
            else
            {
                return null;
            }
        }
    }


    private IPropertyAssigner GetSecurityToSecurityPropertyAssigner()
    {
        var baseAssigner = new MainToStrictPropertyAssigner<TConfiguration>(this);

        var typeReferenceService = new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);

        return this.Configuration.ExpandStrictMaybeToDefault
                       ? new ExpandMaybeSecurityToSecurityPropertyAssigner<TConfiguration>(baseAssigner, typeReferenceService)
                       : baseAssigner.WithSecurityToSecurity(typeReferenceService);
    }




    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       Attributes = MemberAttributes.Public
               };
    }

    protected override CodeExpression GetFieldInitExpression(CodeTypeReference codeTypeReference, PropertyInfo property)
    {
        return this.CodeTypeReferenceService.IsOptional(property) ? this.CodeTypeReferenceService.GetCodeTypeReference(property).ToNothingValueExpression()
               : property.PropertyType.IsCollection() ? (CodeExpression)new CodeObjectCreateExpression(codeTypeReference)
               : property.GetCustomAttribute<DefaultValueAttribute>().Maybe(attr => attr.Value.ToDynamicPrimitiveExpression());
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.IsPersistent())
        {
            yield return this.GetIdentityObjectTypeRef();

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }

            if (this.Configuration.VersionProperty != null)
            {
                yield return this.Configuration.GetVesionObjectCodeTypeReference();
            }
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return this.GenerateDefaultConstructor();

        if (this.HasMapToDomainObjectMethod)
        {
            yield return new BaseMapToDomainObjectMethodFactory<TConfiguration, DefaultStrictDTOFileFactory<TConfiguration>, DTOFileType>(this).GetMethod();
        }

        {
            foreach (var ctor in this.GenerateStrictConstructors())
            {
                yield return ctor;
            }
        }

        if (this.IsPersistent())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            yield return this.GetIdentityObjectImplementation(true);

            if (this.Configuration.VersionProperty != null)
            {
                yield return this.Configuration.GetVersionObjectPrivateImplementation();
            }
        }
    }


    public override IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods()
    {
        foreach (var method in base.GetServerMappingServiceInterfaceMethods())
        {
            yield return method;
        }


        foreach (var masterType in this.Configuration.GetDomainTypeMasters(this.DomainType, this.FileType, true))
        {
            if (this.Configuration.IsPersistentObject(masterType))
            {
                yield return this.GetMappingServiceInterfaceToDomainObjectMethod(masterType);
            }
        }
    }

    public override IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods()
    {
        foreach (var method in base.GetServerMappingServiceMethods())
        {
            yield return method;
        }


        foreach (var masterType in this.Configuration.GetDomainTypeMasters(this.DomainType, this.FileType, true))
        {
            if (this.Configuration.IsPersistentObject(masterType))
            {
                yield return this.GetMappingServiceToDomainObjectMethod(masterType);
            }
        }
    }

    public IEnumerable<CodeMemberMethod> GetClientMappingServiceMethods()
    {
        var propertyAssigner = this.GetSecurityToSecurityPropertyAssigner();
            
        foreach (var sourceFileType in this.GetActualStrictConstructorFileTypes())
        {
            var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");
            var targetParameterRefExpr = targetParameter.ToVariableReferenceExpression();

            var currentSourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, sourceFileType).ToParameterDeclarationExpression("source");
            var currentSourceParameterRefExpr = currentSourceParameter.ToVariableReferenceExpression();

            var properties = this.Configuration.GetDomainTypeProperties(this.DomainType, sourceFileType).Intersect(this.GetProperties(false));

            yield return new CodeMemberMethod
                         {
                                 Name = $"Map{sourceFileType.ShortName}To{this.FileType.ShortName}For{this.DomainType.Name}",
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { targetParameter, currentSourceParameter }
                         }.WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(currentSourceParameter))
                          .WithStatement(new CodeThrowArgumentNullExceptionConditionStatement(targetParameter))
                          .WithStatements(properties.Select(property => propertyAssigner.GetAssignStatementBySource(property, currentSourceParameterRefExpr, targetParameterRefExpr)));
        }
    }

    public IEnumerable<CodeMemberMethod> GetClientMappingServiceInterfaceMethods()
    {
        foreach (var sourceFileType in this.GetActualStrictConstructorFileTypes())
        {
            var targetParameter = this.CurrentReference.ToParameterDeclarationExpression("target");

            var sourceParameter = this.Configuration.GetCodeTypeReference(this.DomainType, sourceFileType).ToParameterDeclarationExpression("currentSource");

            yield return new CodeMemberMethod
                         {
                                 Name = $"Map{sourceFileType.ShortName}To{this.FileType.ShortName}For{this.DomainType.Name}",
                                 Parameters = { targetParameter, sourceParameter }
                         };
        }
    }
}
