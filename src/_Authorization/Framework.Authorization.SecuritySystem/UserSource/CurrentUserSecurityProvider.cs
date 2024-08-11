using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class CurrentUserSecurityProvider<TDomainObject>(
    ICurrentUserSource currentUserSource,
    IUserPathInfoRelativeService userPathInfoRelativeService) : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =
        userPathInfoRelativeService.GetId<TDomainObject>().Select(id => id == currentUserSource.CurrentUserId);
    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        return SecurityAccessorData.TryReturn(userPathInfoRelativeService.GetName<TDomainObject>().Eval(domainObject));
    }
}
