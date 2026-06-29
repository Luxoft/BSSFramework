using Anch.SecuritySystem.Configurator.Interfaces;

using Framework.Application.Domain;
using Framework.Database;

using Microsoft.Extensions.Logging;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) : IConfiguratorIntegrationEvents
{
    public Task PrincipalCreatedAsync(object principal, CancellationToken ct)
    {
        logger.LogInformation("Principal with {Id} has been created", ((IIdentityObject<Guid>)principal).Id);

        return Task.CompletedTask;
    }

    public Task PrincipalRemovedAsync(object principal, CancellationToken ct) => Task.CompletedTask;

    public Task PrincipalChangedAsync(object principal, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionRemovedAsync(object permission, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionCreatedAsync(object permission, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionChangedAsync(object permission, CancellationToken ct) => Task.CompletedTask;
}

