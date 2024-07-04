using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class IntegrationMethodGenerator<TConfiguration, TBLLRoleAttribute> : MethodGenerator<TConfiguration, TBLLRoleAttribute>
    where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
    where TBLLRoleAttribute : BLLServiceRoleAttribute
{
    protected IntegrationMethodGenerator(TConfiguration configuration, Type domainType)
        : base(configuration, domainType)
    {
    }

    protected override SecurityRule SecurityRule { get; } = SecurityRule.Disabled;


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return evaluateDataExpr.GetContext()
                                     .ToPropertyReference("Authorization")
                                     .ToPropertyReference("AuthorizationSystem")
                                     .ToMethodInvokeExpression("CheckAccess", this.Configuration.IntegrationSecurityRule)
                                     .ToExpressionStatement();
    }
}
