using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUserSource<TUser>(ICurrentUser currentUser, IUserSource<TUser> userSource) : ICurrentUserSource<TUser>
{
    private readonly Lazy<TUser> lazyCurrentUser = LazyHelper.Create(() => userSource.GetByName(currentUser.Name));

    public TUser CurrentUser => this.lazyCurrentUser.Value;
}
