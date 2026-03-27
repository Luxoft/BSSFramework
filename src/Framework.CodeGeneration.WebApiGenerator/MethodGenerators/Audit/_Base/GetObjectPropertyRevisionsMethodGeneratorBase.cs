using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.ServiceModel.Service;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.WebApiGenerator.Configuration.Audit;
using Framework.CodeGeneration.WebApiGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Audit._Base;

public abstract class GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    private readonly IAuditDTOGeneratorConfigurationBase dtoConfiguration;

    protected GetObjectPropertyRevisionsMethodGeneratorBase(TConfiguration configuration, Type domainType, IAuditDTOGeneratorConfigurationBase dtoConfiguration)
            : base(configuration, domainType)
    {
        this.dtoConfiguration = dtoConfiguration;
    }


    protected override CodeTypeReference ReturnType => new(this.dtoConfiguration.DomainObjectPropertiesRevisionDTOFullTypeName);

    protected override bool IsEdit { get; } = false;


    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.PropertyNameParameter;
    }

    private CodeParameterDeclarationExpression PropertyNameParameter => new(typeof(string), "propertyName");

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var auditServiceCodeReference = new CodeTypeReference(typeof(AuditService<,,,,,,>))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.GetIdentityType()))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLFactoryInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.ActualRootSecurityServiceInterfaceType))

                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.PersistentDomainObjectBaseType))
                                        .Self(z => z.TypeArguments.Add(this.dtoConfiguration.DomainObjectPropertiesRevisionDTOFullTypeName))
                                        .Self(z => z.TypeArguments.Add(this.dtoConfiguration.PropertyRevisionFullTypeName));

        yield return auditServiceCodeReference
                     .ToObjectCreateExpression(evaluateDataExpr.GetContext())
                     .ToMethodInvokeExpression("GetPropertyChanges", this.GetBLLMethodParameters().ToArray())
                     .Self(z => z.Method.TypeArguments.Add(this.DomainType))
                     .ToMethodReturnStatement();
    }

    protected virtual IEnumerable<CodeExpression> GetBLLMethodParameters()
    {
        yield return this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty);
        yield return this.PropertyNameParameter.ToVariableReferenceExpression();
    }
}
