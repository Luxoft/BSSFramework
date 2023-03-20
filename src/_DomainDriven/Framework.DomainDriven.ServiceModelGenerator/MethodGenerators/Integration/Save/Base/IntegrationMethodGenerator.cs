using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

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
        var checkAccessMethod = typeof(AuthorizationBLLContextExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("CheckAccess");

        yield return evaluateDataExpr.GetContext().ToPropertyReference("Authorization")
                                     .ToStaticMethodInvokeExpression(checkAccessMethod, this.Configuration.IntegrationSecurityOperation)
                                     .ToExpressionStatement();
    }
}
