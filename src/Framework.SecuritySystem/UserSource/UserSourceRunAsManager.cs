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
    [DisabledSecurity] IPersistStorage<TUser> persistStorage) : RunAsManager(userAuthenticationService, securitySystemFactory)
{
    private readonly TUser currentUser = userSource.TryGetByName(userAuthenticationService.GetUserName())!;

    public override string? RunAsName => accessor.GetRunAs(this.currentUser).Maybe(v => userPathInfo.NamePath.Eval(v));

    protected override async Task PersistRunAs(string? principalName, CancellationToken cancellationToken)
    {
        var runAsValue = principalName == null
                             ? default
                             : userSource.TryGetByName(principalName) ?? throw new Exception($"User \"{principalName}\" not found");

        accessor.SetRunAs(this.currentUser, runAsValue);;

        await persistStorage.SaveAsync(this.currentUser, cancellationToken);
    }
}
