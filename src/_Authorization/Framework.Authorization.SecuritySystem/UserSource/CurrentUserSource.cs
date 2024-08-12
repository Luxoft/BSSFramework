using Framework.Core;

namespace Framework.Authorization.SecuritySystem.UserSource;

public class CurrentUserSource<TUserDomainObject> : ICurrentUserSource<TUserDomainObject>
{
    private readonly Lazy<TUserDomainObject> lazyCurrentUser;

    private readonly Lazy<Guid> lazyCurrentUserId;

    private readonly Lazy<string> lazyCurrentUserName;

    public CurrentUserSource(
        IUserSource<TUserDomainObject> userSource,
        UserPathInfo<TUserDomainObject> userPathInfo,
        IActualPrincipalSource actualPrincipalSource)
    {
        this.lazyCurrentUser = LazyHelper.Create(
            () => userSource.TryGetByName(actualPrincipalSource.ActualPrincipal.Name)
                  ?? throw new Exception(
                      $"{typeof(TUserDomainObject).Name} with {userPathInfo.NamePath.GetProperty().Name} ({actualPrincipalSource.ActualPrincipal.Name}) not found"));

        this.lazyCurrentUserId = LazyHelper.Create(() => userPathInfo.IdPath.Eval(this.CurrentUser));

        this.lazyCurrentUserName = LazyHelper.Create(() => userPathInfo.NamePath.Eval(this.CurrentUser));
    }

    public TUserDomainObject CurrentUser => this.lazyCurrentUser.Value;

    public Guid CurrentUserId => this.lazyCurrentUserId.Value;

    public string CurrentUserName => this.lazyCurrentUserName.Value;
}
