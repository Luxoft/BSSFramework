using Framework.Core.Services;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem.Services;

public abstract class RunAsManager(IUserAuthenticationService userAuthenticationService, ISecuritySystemFactory securitySystemFactory)
    : IRunAsManager
{
    public abstract User? RunAsUser { get; }

    private UserCredential PureCredential { get; } = userAuthenticationService.GetUserName();

    public async Task StartRunAsUserAsync(UserCredential userCredential, CancellationToken cancellationToken)
    {
        this.CheckAccess();

        if (this.RunAsUser != null && userCredential.IsMatch(this.RunAsUser))
        {
        }
        else if (userCredential == this.PureCredential)
        {
            await this.FinishRunAsUserAsync(cancellationToken);
        }
        else
        {
            await this.PersistRunAs(userCredential, cancellationToken);
        }
    }

    public async Task FinishRunAsUserAsync(CancellationToken cancellationToken)
    {
        this.CheckAccess();

        await this.PersistRunAs(null, cancellationToken);
    }

    protected abstract Task PersistRunAs(UserCredential? userCredential, CancellationToken cancellationToken);

    private void CheckAccess() =>
        securitySystemFactory.Create(new SecurityRuleCredential.CurrentUserWithoutRunAsCredential())
                             .CheckAccess(SecurityRole.Administrator);
}
