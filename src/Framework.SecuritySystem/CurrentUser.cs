using Framework.Core;
using Framework.Core.Services;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem;

public class CurrentUser : ICurrentUser
{
    private readonly IRunAsManager? runAsManager;

    private readonly Lazy<string> lazyName;

    private readonly Lazy<Guid> lazyId;

    public CurrentUser(
        IUserAuthenticationService userAuthenticationService,
        IRunAsManager? runAsManager = null,
        IUserIdentitySource? userIdentitySource = null)
    {
        this.runAsManager = runAsManager;
        this.lazyName = LazyHelper.Create(userAuthenticationService.GetUserName);

        this.lazyId = LazyHelper.Create(
            () => (userIdentitySource ?? throw new UserSourceException($"{nameof(UserSource)} not defined"))
                  .TryGetId(this.Name)!.Value);
    }

    public Guid Id => this.lazyId.Value;

    public string Name => this.runAsManager?.RunAsName ?? this.lazyName.Value;
}
