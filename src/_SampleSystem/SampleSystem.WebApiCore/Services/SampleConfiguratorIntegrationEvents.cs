using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;

using Microsoft.Extensions.Logging;

namespace SampleSystem.WebApiCore.Services;

public class SampleConfiguratorIntegrationEvents : IConfiguratorIntegrationEvents
{
    private readonly ILogger<SampleConfiguratorIntegrationEvents> logger;

    public SampleConfiguratorIntegrationEvents(ILogger<SampleConfiguratorIntegrationEvents> logger) => this.logger = logger;

    public Task PrincipalCreatedAsync(Principal principal, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Principal {Name} has been created", principal.Name);
        return Task.CompletedTask;
    }

    public Task PrincipalRemovedAsync(Principal principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PrincipalChangedAsync(Principal principal, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionRemovedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionCreatedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PermissionChangedAsync(Permission permission, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task BusinessRoleCreatedAsync(BusinessRole businessRole, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task BusinessRoleChangedAsync(BusinessRole businessRole, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task BusinessRoleRemovedAsync(BusinessRole businessRole, CancellationToken cancellationToken) => Task.CompletedTask;
}
