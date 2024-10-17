using Framework.Core;
using Framework.Core.Services;
using Framework.SecuritySystem.Credential;
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
    private readonly TUser currentUser = userSource.GetUser(userAuthenticationService.GetUserName());

    public override User? RunAsUser => accessor.GetRunAs(this.currentUser).Maybe(v => userPathInfo.ToDefaultUserExpr.Eval(v));

    protected override async Task PersistRunAs(UserCredential? userCredential, CancellationToken cancellationToken)
    {
        var runAsUser = userCredential == null ? default : userSource.GetUser(userCredential);

        accessor.SetRunAs(this.currentUser, runAsUser);

        await persistStorage.SaveAsync(this.currentUser, cancellationToken);
    }
}
