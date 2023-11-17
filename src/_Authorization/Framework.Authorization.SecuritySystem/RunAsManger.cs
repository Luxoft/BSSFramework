using Framework.Authorization.Domain;

using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class RunAsManger : IRunAsManager
{
    private readonly IOperationAccessorFactory operationAccessorFactory;

    private readonly IRepository<Principal> principalRepository;

    private readonly Principal currentPrincipal;

    public RunAsManger(
        [FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<Principal> principalRepository,
        ICurrentPrincipalSource currentPrincipalSource,
        IOperationAccessorFactory operationAccessorFactory)
    {
        this.operationAccessorFactory = operationAccessorFactory;
        this.principalRepository = principalRepository;

        this.currentPrincipal =  currentPrincipalSource.CurrentPrincipal;
    }



    public bool IsRunningAs => this.currentPrincipal.RunAs != null;


    public async Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken)
    {
        if (principalName == null) throw new ArgumentNullException(nameof(principalName));

        this.operationAccessorFactory.Create(false).CheckAccess(AuthorizationSecurityOperation.AuthorizationImpersonate);

        if (string.Equals(principalName, this.currentPrincipal.RunAs?.Name, StringComparison.CurrentCultureIgnoreCase))
        {

        }
        else if (string.Equals(principalName, this.currentPrincipal.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            await this.FinishRunAsUserAsync(cancellationToken);
        }
        else
        {
            this.currentPrincipal.RunAs = this.principalRepository.GetQueryable().SingleOrDefault(p => p.Name == principalName)
                                          ?? throw new Exception($"Principal with name '{principalName}' not found");

            await this.principalRepository.SaveAsync(this.currentPrincipal, cancellationToken);
        }
    }

    public async Task FinishRunAsUserAsync(CancellationToken cancellationToken)
    {
        this.currentPrincipal.RunAs = null;

        await this.principalRepository.SaveAsync(this.currentPrincipal, cancellationToken);
    }
}
