using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class RunAsManger(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    ICurrentPrincipalSource currentPrincipalSource,
    IAuthorizationSystemFactory authorizationSystemFactory)
    : IRunAsManager
{
    private Principal CurrentPrincipal => currentPrincipalSource.CurrentPrincipal;

    public bool IsRunningAs => this.CurrentPrincipal.RunAs != null;

    public async Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken)
    {
        this.CheckAccess();

        if (string.Equals(principalName, this.CurrentPrincipal.RunAs?.Name, StringComparison.CurrentCultureIgnoreCase))
        {
        }
        else if (string.Equals(principalName, this.CurrentPrincipal.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            await this.FinishRunAsUserAsync(cancellationToken);
        }
        else
        {
            this.CurrentPrincipal.RunAs = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == principalName)
                                          ?? throw new Exception($"Principal with name '{principalName}' not found");

            await principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
        }
    }

    public async Task FinishRunAsUserAsync(CancellationToken cancellationToken)
    {
        this.CheckAccess();

        this.CurrentPrincipal.RunAs = null;

        await principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
    }

    private void CheckAccess() => authorizationSystemFactory.Create(false).CheckAccess(SecurityRole.Administrator);
}
