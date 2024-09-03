using Framework.Core;
using Framework.Core.Services;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUser(IUserAuthenticationService userAuthenticationService, IRunAsManager? runAsManager = null) : ICurrentUser
{
    private readonly Lazy<string> lazyName = LazyHelper.Create(userAuthenticationService.GetUserName);

    public string Name => runAsManager?.RunAsName ?? this.lazyName.Value;
}
