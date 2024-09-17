using Framework.Persistent;

namespace Framework.Configurator.Interfaces;

public interface IConfiguratorIntegrationEvents
{
    Task PrincipalCreatedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken = default);

    Task PrincipalChangedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken = default);

    Task PrincipalRemovedAsync(IIdentityObject<Guid> principal, CancellationToken cancellationToken = default);

    Task PermissionCreatedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken = default);

    Task PermissionChangedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken = default);

    Task PermissionRemovedAsync(IIdentityObject<Guid> permission, CancellationToken cancellationToken = default);
}
