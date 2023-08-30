using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator.Audit;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    private readonly IAuditDTOGeneratorConfigurationBase dtoConfiguration;

    protected GetObjectPropertyRevisionsMethodGeneratorBase(TConfiguration configuration, Type domainType, IAuditDTOGeneratorConfigurationBase dtoConfiguration)
            : base(configuration, domainType)
    {
        this.dtoConfiguration = dtoConfiguration;
    }


    protected override CodeTypeReference ReturnType => new CodeTypeReference(this.dtoConfiguration.DomainObjectPropertiesRevisionDTOFullTypeName);

    protected override bool IsEdit { get; } = false;


    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.PropertyNameParameter;
    }

    private CodeParameterDeclarationExpression PropertyNameParameter => new CodeParameterDeclarationExpression(typeof(string), "propertyName");

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var auditServiceCodeReference = new CodeTypeReference(typeof(AuditService<,,,,,,,>))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.GetIdentityType()))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.BLLFactoryInterfaceTypeReference))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.BLLCore.RootSecurityServiceInterface))
                                        .Self(z => z.TypeArguments.Add(this.Configuration.Environment.SecurityOperationCodeType))

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
