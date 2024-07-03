using System.Linq.Expressions;

using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem;

namespace SampleSystem.Security.Services;

public class CurrentEmployeeSecurityProvider<TDomainObject>(
    IActualPrincipalSource actualPrincipalSource,
    ToEmployeePathInfo<TDomainObject> toEmployeePathInfo) : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =
        toEmployeePathInfo.Path.Select(employee => employee.Login == actualPrincipalSource.ActualPrincipal.Name);

    public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
    {
        return UnboundedList.Yeild(toEmployeePathInfo.Path.Eval(domainObject).Login);
    }
}
