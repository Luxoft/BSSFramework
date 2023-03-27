using Framework.Authorization.Domain;

namespace Framework.Configurator.Interfaces;

public interface IConfiguratorIntegrationEvents
{
    Task PrincipalCreatedAsync(Principal principal, CancellationToken cancellationToken);

    Task PrincipalRemovedAsync(Principal principal, CancellationToken cancellationToken);

    Task PrincipalChangedAsync(Principal principal, CancellationToken cancellationToken);

    Task PermissionRemovedAsync(Permission permission, CancellationToken cancellationToken);

    Task PermissionCreatedAsync(Permission permission, CancellationToken cancellationToken);

    Task PermissionChangedAsync(Permission permission, CancellationToken cancellationToken);

    Task BusinessRoleCreatedAsync(BusinessRole businessRole, CancellationToken cancellationToken);

    Task BusinessRoleChangedAsync(BusinessRole businessRole, CancellationToken cancellationToken);

    Task BusinessRoleRemovedAsync(BusinessRole businessRole, CancellationToken cancellationToken);
}
