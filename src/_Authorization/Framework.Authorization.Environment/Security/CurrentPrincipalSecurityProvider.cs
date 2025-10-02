using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;
using SecuritySystem;
using SecuritySystem.SecurityAccessor;

namespace Framework.Authorization.Environment.Security;

public class CurrentPrincipalSecurityProvider<TDomainObject>(
    ICurrentPrincipalSource currentPrincipalSource,
    IRelativeDomainPathInfo<TDomainObject, Principal> toPrincipalPathInfo)
    : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        toPrincipalPathInfo.CreateCondition(principal => principal == currentPrincipalSource.CurrentPrincipal);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        SecurityAccessorData.Return(currentPrincipalSource.CurrentPrincipal.Name);
}
