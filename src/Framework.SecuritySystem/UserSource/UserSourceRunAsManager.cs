using Framework.Core;
using Framework.Core.Services;
using Framework.SecuritySystem.PersistStorage;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.UserSource;

public class UserSourceRunAsManager<TUser>(
    IUserAuthenticationService userAuthenticationService,
    ISecuritySystemFactory securitySystemFactory,
    IUserSource<TUser> userSource,
    IUserSourceRunAsAccessor<TUser> accessor,
    UserPathInfo<TUser> userPathInfo,
    IPersistStorage<TUser> persistStorage) : RunAsManager(userAuthenticationService, securitySystemFactory)
{
    private readonly TUser currentUser = userSource.GetByName(userAuthenticationService.GetUserName());

    public override string? RunAsName => accessor.GetRunAs(this.currentUser).Maybe(v => userPathInfo.NamePath.Eval(v));

    protected override async Task PersistRunAs(string? principalName, CancellationToken cancellationToken)
    {
        var runAsValue = principalName == null ? default : userSource.GetByName(principalName);

        accessor.SetRunAs(this.currentUser, runAsValue);

        await persistStorage.SaveAsync(this.currentUser, cancellationToken);
    }
}
