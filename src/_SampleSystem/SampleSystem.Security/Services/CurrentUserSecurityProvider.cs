using System.Linq.Expressions;

using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class CurrentUserSecurityProvider<TDomainObject>(
    IActualPrincipalSource actualPrincipalSource,
    IRelativeDomainPathInfo<TDomainObject, Employee> toEmployeePathInfo) : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =
        toEmployeePathInfo.Path.Select(employee => employee.Active && employee.Login == actualPrincipalSource.ActualPrincipal.Name);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        return SecurityAccessorData.TryReturn(toEmployeePathInfo.Path.Eval(domainObject).Login);
    }
}
