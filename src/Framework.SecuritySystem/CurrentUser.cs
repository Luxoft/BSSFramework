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
        IUserSource? userSource = null)
    {
        this.runAsManager = runAsManager;
        this.lazyName = LazyHelper.Create(userAuthenticationService.GetUserName);

        this.lazyId = LazyHelper.Create(
            () => (userSource ?? throw new UserSourceException($"{nameof(UserSource)} not defined")).GetUser(this.Name).Id);
    }

    public Guid Id => this.lazyId.Value;

    public string Name => this.runAsManager?.RunAsUser?.Name ?? this.lazyName.Value;
}
