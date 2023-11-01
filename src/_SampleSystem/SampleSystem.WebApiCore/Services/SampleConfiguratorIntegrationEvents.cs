using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents : IConfiguratorIntegrationEvents
{
    private readonly ILogger<SampleConfiguratorIntegrationEvents> logger;

    public SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) => this.logger = logger;

    public async Task PrincipalCreatedAsync(Principal principal, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Principal {Name} has been created", principal.Name);
    }

    public async Task PrincipalRemovedAsync(Principal principal, CancellationToken cancellationToken)
    {
    }

    public async Task PrincipalChangedAsync(Principal principal, CancellationToken cancellationToken)
    {
    }

    public async Task PermissionRemovedAsync(Permission permission, CancellationToken cancellationToken)
    {
    }

    public async Task PermissionCreatedAsync(Permission permission, CancellationToken cancellationToken)
    {
    }

    public async Task PermissionChangedAsync(Permission permission, CancellationToken cancellationToken)
    {
    }

    public async Task BusinessRoleCreatedAsync(BusinessRole businessRole, CancellationToken cancellationToken)
    {
    }

    public async Task BusinessRoleChangedAsync(BusinessRole businessRole, CancellationToken cancellationToken)
    {
    }

    public async Task BusinessRoleRemovedAsync(BusinessRole businessRole, CancellationToken cancellationToken)
    {
    }
}
