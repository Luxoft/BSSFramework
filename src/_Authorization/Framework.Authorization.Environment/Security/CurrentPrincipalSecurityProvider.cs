using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;

using SecuritySystem.Providers;
using SecuritySystem.RelativeDomainPathInfo;
using SecuritySystem.SecurityAccessor;

namespace Framework.Authorization.Environment.Security;

public class CurrentPrincipalSecurityProvider<TDomainObject>(
    IExpressionEvaluator expressionEvaluator,
    ICurrentPrincipalSource currentPrincipalSource,
    IRelativeDomainPathInfo<TDomainObject, Principal> toPrincipalPathInfo)
    : SecurityProvider<TDomainObject>(expressionEvaluator)
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        toPrincipalPathInfo.CreateCondition(principal => principal == currentPrincipalSource.CurrentPrincipal);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        SecurityAccessorData.Return(currentPrincipalSource.CurrentPrincipal.Name);
}
