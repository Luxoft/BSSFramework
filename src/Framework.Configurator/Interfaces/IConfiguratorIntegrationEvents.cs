namespace Framework.Configurator.Interfaces;

public interface IConfiguratorIntegrationEvents
{
    Task PrincipalCreatedAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task PrincipalChangedAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task PrincipalRemovedAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task PermissionCreatedAsync(Guid permissionId, CancellationToken cancellationToken = default);

    Task PermissionChangedAsync(Guid permissionId, CancellationToken cancellationToken = default);

    Task PermissionRemovedAsync(Guid permissionId, CancellationToken cancellationToken = default);
}
