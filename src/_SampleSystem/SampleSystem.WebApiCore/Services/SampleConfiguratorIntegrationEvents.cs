using Framework.Application.Domain;
using Framework.Database;

using Microsoft.Extensions.Logging;

using SecuritySystem.Configurator.Interfaces;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger, ICurrentRevisionService currentRevisionService) : IConfiguratorIntegrationEvents
{
    public Task PrincipalCreatedAsync(object principal, CancellationToken cancellationToken)
    {
        logger.LogInformation("Principal with {Id} has been created", ((IIdentityObject<Guid>)principal).Id);

        return Task.CompletedTask;
    }

    public Task PrincipalRemovedAsync(object principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(object principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(object permission, CancellationToken cancellationToken) => Task.CompletedTask;
}
