using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class RunAsManger : IRunAsManager
{
    private readonly IOperationAccessorFactory operationAccessorFactory;

    private readonly IRepository<Principal> principalRepository;

    private readonly ICurrentPrincipalSource currentPrincipalSource;

    public RunAsManger(
        [FromKeyedServices(SecurityRule.Disabled)] IRepository<Principal> principalRepository,
        ICurrentPrincipalSource currentPrincipalSource,
        IOperationAccessorFactory operationAccessorFactory)
    {
        this.operationAccessorFactory = operationAccessorFactory;
        this.principalRepository = principalRepository;
        this.currentPrincipalSource = currentPrincipalSource;
    }

    private Principal CurrentPrincipal => this.currentPrincipalSource.CurrentPrincipal;


    public bool IsRunningAs => this.CurrentPrincipal.RunAs != null;


    public async Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken)
    {
        if (principalName == null) throw new ArgumentNullException(nameof(principalName));

        this.operationAccessorFactory.Create(false).CheckAccess(AuthorizationSecurityOperation.AuthorizationImpersonate);

        if (string.Equals(principalName, this.CurrentPrincipal.RunAs?.Name, StringComparison.CurrentCultureIgnoreCase))
        {

        }
        else if (string.Equals(principalName, this.CurrentPrincipal.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            await this.FinishRunAsUserAsync(cancellationToken);
        }
        else
        {
            this.CurrentPrincipal.RunAs = this.principalRepository.GetQueryable().SingleOrDefault(p => p.Name == principalName)
                                          ?? throw new Exception($"Principal with name '{principalName}' not found");

            await this.principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
        }
    }

    public async Task FinishRunAsUserAsync(CancellationToken cancellationToken)
    {
        this.CurrentPrincipal.RunAs = null;

        await this.principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
    }
}
