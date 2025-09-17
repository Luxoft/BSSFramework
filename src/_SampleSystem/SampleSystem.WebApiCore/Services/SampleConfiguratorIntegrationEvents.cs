using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.Persistent;

using Microsoft.Extensions.Logging;

using SecuritySystem.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger, ICurrentRevisionService currentRevisionService) : IConfiguratorIntegrationEvents
{
    public async Task PrincipalCreatedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken)
    {
        var z = currentRevisionService.GetCurrentRevision();
        logger.LogInformation("Principal with {Id} has been created", principal.Id);
    }

    public Task PrincipalRemovedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken) => Task.CompletedTask;
}
