using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) : IConfiguratorIntegrationEvents
{
    public async Task PrincipalCreatedAsync(Guid principal, CancellationToken cancellationToken)
    {
        logger.LogInformation("Principal {Name} has been created", principal);
    }

    public Task PrincipalRemovedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;
}
