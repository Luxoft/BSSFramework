using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class CurrentUserSecurityProvider<TDomainObject, TUserDomainObject>(
    ICurrentUserSource<TUserDomainObject> currentUserSource,
    IRelativeDomainPathInfo<TDomainObject, TUserDomainObject> toUserPathInfo,
    UserPathInfo<TUserDomainObject> userPathInfo) : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =
        userPathInfo.IdPath.Eval(currentUserSource.CurrentUser).Pipe(
            currentUserId =>
                toUserPathInfo.Path.Select(userPathInfo.IdPath).Select(userId => userId == currentUserId));
    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        return SecurityAccessorData.TryReturn(toUserPathInfo.Select(userPathInfo.NamePath).Path.Eval(domainObject));
    }
}
