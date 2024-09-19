using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUserSource<TUser> : ICurrentUserSource<TUser>
{
    private readonly Lazy<TUser> lazyCurrentUser;

    private readonly Lazy<Guid> lazyCurrentUserId;

    public CurrentUserSource(
        IUserSource<TUser> userSource,
        UserPathInfo<TUser> userPathInfo,
        ICurrentUser currentUser)
    {
        this.lazyCurrentUser = LazyHelper.Create(
            () => userSource.TryGetByName(currentUser.Name)
                  ?? throw new Exception(
                      $"{typeof(TUser).Name} with {userPathInfo.NamePath.GetProperty().Name} ({currentUser.Name}) not found"));

        this.lazyCurrentUserId = LazyHelper.Create(() => userPathInfo.IdPath.Eval(this.CurrentUser));
    }

    public TUser CurrentUser => this.lazyCurrentUser.Value;

    public Guid CurrentUserId => this.lazyCurrentUserId.Value;
}
