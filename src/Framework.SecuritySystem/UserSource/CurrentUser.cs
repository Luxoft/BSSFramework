using Framework.Core;
using Framework.Core.Services;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUser(IUserAuthenticationService userAuthenticationService) : ICurrentUser
{
    private readonly Lazy<string> lazyCurrentUserIdentity = LazyHelper.Create(userAuthenticationService.GetUserName);

    public string CurrentUserIdentity => this.lazyCurrentUserIdentity.Value;

    public string Name => this.lazyCurrentUserIdentity.Value;
}
