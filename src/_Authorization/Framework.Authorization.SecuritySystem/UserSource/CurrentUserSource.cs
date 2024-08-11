using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class CurrentUserSource<TUserDomainObject>(
    [DisabledSecurity] IRepository<TUserDomainObject> userRepository,
    UserPathInfo<TUserDomainObject> userPathInfo,
    IActualPrincipalSource actualPrincipalSource)
    : ICurrentUserSource<TUserDomainObject>
{
    private readonly Lazy<TUserDomainObject> lazyCurrentUser =
        LazyHelper.Create(
            () => userRepository.GetQueryable()
                                .Where(userPathInfo.Filter)
                                .Where(userPathInfo.NamePath.Select(name => name == actualPrincipalSource.ActualPrincipal.Name))
                                .Single());

    public TUserDomainObject CurrentUser => this.lazyCurrentUser.Value;

    public class CurrentUserSecurityProvider<TDomainObject>(
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

    public static Type CurrentUserSecurityProviderGenericType { get; } = typeof(CurrentUserSecurityProvider<>);
}
