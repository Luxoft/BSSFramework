using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class CurrentUserSource<TUserDomainObject> : ICurrentUserSource<TUserDomainObject>
{
    private readonly Lazy<TUserDomainObject> lazyCurrentUser;

    private readonly Lazy<Guid> lazyCurrentUserId;

    private readonly Lazy<string> lazyCurrentUserName;

    public CurrentUserSource(
        [DisabledSecurity] IRepository<TUserDomainObject> userRepository,
        UserPathInfo<TUserDomainObject> userPathInfo,
        IActualPrincipalSource actualPrincipalSource)
    {
        this.lazyCurrentUser = LazyHelper.Create(
            () => userRepository.GetQueryable()
                                .Where(userPathInfo.Filter)
                                .Where(userPathInfo.NamePath.Select(name => name == actualPrincipalSource.ActualPrincipal.Name))
                                .Single());

        this.lazyCurrentUserId = LazyHelper.Create(() => userPathInfo.IdPath.Eval(this.CurrentUser));

        this.lazyCurrentUserName = LazyHelper.Create(() => userPathInfo.NamePath.Eval(this.CurrentUser));
    }

    public TUserDomainObject CurrentUser => this.lazyCurrentUser.Value;

    public Guid CurrentUserId => this.lazyCurrentUserId.Value;

    public string CurrentUserName => this.lazyCurrentUserName.Value;
}
