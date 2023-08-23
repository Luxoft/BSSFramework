using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class IntegrationMethodGenerator<TConfiguration, TBLLRoleAttribute> : MethodGenerator<TConfiguration, TBLLRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TBLLRoleAttribute : BLLServiceRoleAttribute
{
    protected IntegrationMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    protected sealed override bool RequiredSecurity { get; } = false;

    protected override bool IsEdit { get; } = true;


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return evaluateDataExpr.GetContext().ToPropertyReference("Authorization")
                                     .ToMethodInvokeExpression("CheckAccess", this.Configuration.IntegrationSecurityOperation)
                                     .ToExpressionStatement();
    }
}
