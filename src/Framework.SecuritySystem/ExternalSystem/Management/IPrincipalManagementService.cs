using Framework.Core;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public interface IPrincipalService
{
    Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(string nameFilter, int limit, CancellationToken cancellationToken = default);

    Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetLinkedPrincipalsAsync(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken = default);
}

public interface IPrincipalManagementService : IPrincipalService
{
    Task<Guid> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default);

    Task UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken);

    Task RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<MergeResult<Guid, Guid>> UpdatePermissionsAsync(Guid principalId, IEnumerable<TypedPermission> typedPermissions, CancellationToken cancellationToken = default);
}
