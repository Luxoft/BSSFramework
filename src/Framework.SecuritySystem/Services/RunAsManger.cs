using Framework.Core.Services;

namespace Framework.SecuritySystem.Services;

public abstract class RunAsManager( IUserAuthenticationService userAuthenticationService) : IRunAsManager
{
    public abstract string? RunAsName { get; }

    private string? PureName => userAuthenticationService.GetUserName();

    public async Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken)
    {
        this.CheckAccess();

        if (string.Equals(principalName, this.RunAsName, StringComparison.CurrentCultureIgnoreCase))
        {
        }
        else if (string.Equals(principalName, this.PureName, StringComparison.CurrentCultureIgnoreCase))
        {
            await this.FinishRunAsUserAsync(cancellationToken);
        }
        else
        {
            await this.PersistRunAs(principalName, cancellationToken);
        }
    }

    public async Task FinishRunAsUserAsync(CancellationToken cancellationToken)
    {
        this.CheckAccess();

        await this.PersistRunAs(null, cancellationToken);
    }

    protected abstract Task PersistRunAs(string? principalName, CancellationToken cancellationToken);

    protected abstract void CheckAccess();
}
