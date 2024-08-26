using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUserSource<TUserDomainObject> : ICurrentUserSource<TUserDomainObject>
{
    private readonly Lazy<TUserDomainObject> lazyCurrentUser;

    private readonly Lazy<Guid> lazyCurrentUserId;

    public CurrentUserSource(
        IUserSource<TUserDomainObject> userSource,
        UserPathInfo<TUserDomainObject> userPathInfo,
        ICurrentUser currentUser)
    {
        this.lazyCurrentUser = LazyHelper.Create(
            () => userSource.TryGetByName(currentUser.Name)
                  ?? throw new Exception(
                      $"{typeof(TUserDomainObject).Name} with {userPathInfo.NamePath.GetProperty().Name} ({currentUser.Name}) not found"));

        this.lazyCurrentUserId = LazyHelper.Create(() => userPathInfo.IdPath.Eval(this.CurrentUser));
    }

    public TUserDomainObject CurrentUser => this.lazyCurrentUser.Value;

    public Guid CurrentUserId => this.lazyCurrentUserId.Value;
}
