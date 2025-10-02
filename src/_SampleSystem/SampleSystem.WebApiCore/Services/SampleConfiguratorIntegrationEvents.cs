using Framework.DomainDriven;
using Framework.Persistent;

using Microsoft.Extensions.Logging;

using SecuritySystem.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger, ICurrentRevisionService currentRevisionService) : IConfiguratorIntegrationEvents
{
    public async Task PrincipalCreatedAsync(object principal, CancellationToken cancellationToken)
    {
        var z = currentRevisionService.GetCurrentRevision();
        
        logger.LogInformation("Principal with {Id} has been created", ((IIdentityObject<Guid>)principal).Id);
    }

    public Task PrincipalRemovedAsync(object principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(object principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;
}
