using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public interface IPrincipalService
{
    Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(string nameFilter, int limit, CancellationToken cancellationToken = default);

    Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetLinkedPrincipalsAsync(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken = default);
}

public interface IPrincipalManagementService : IPrincipalService
{
    Task<IIdentityObject<Guid>> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default);

    Task<IIdentityObject<Guid>> UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken);

    Task<IIdentityObject<Guid>> RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>> UpdatePermissionsAsync(Guid principalId, IEnumerable<TypedPermission> typedPermissions, CancellationToken cancellationToken = default);
}
