using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public class CurrentPrincipalSecurityProvider<TDomainObject>(
    ICurrentPrincipalSource currentPrincipalSource,
    IRelativeDomainPathInfo<TDomainObject, Principal> toPrincipalPathInfo)
    : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        ExpressionHelper.Create((Principal principal) => principal == currentPrincipalSource.CurrentPrincipal)
                        .OverrideInput(toPrincipalPathInfo.Path);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        SecurityAccessorData.Return(currentPrincipalSource.CurrentPrincipal.Name);
}
