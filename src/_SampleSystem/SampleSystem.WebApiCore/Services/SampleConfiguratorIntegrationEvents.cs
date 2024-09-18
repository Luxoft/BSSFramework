using Framework.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) : IConfiguratorIntegrationEvents
{
    public async Task PrincipalCreatedAsync(Guid principalId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Principal with {Id} has been created", principalId);
    }

    public Task PrincipalRemovedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(Guid principalId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(Guid permissionId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(Guid permissionId, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(Guid permissionId, CancellationToken cancellationToken) => Task.CompletedTask;
}
