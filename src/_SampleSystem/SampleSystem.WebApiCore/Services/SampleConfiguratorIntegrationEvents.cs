using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) : IConfiguratorIntegrationEvents
{
    public Task PrincipalCreatedAsync(Principal principal, CancellationToken cancellationToken)
    {
        logger.LogInformation("Principal {Name} has been created", principal.Name);
        return Task.CompletedTask;
    }

    public Task PrincipalRemovedAsync(Principal principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(Principal principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;
}
