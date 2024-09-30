using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public interface IPrincipalManagementService
{
    Task<IIdentityObject<Guid>> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default);

    Task<IIdentityObject<Guid>> UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken);

    Task<IIdentityObject<Guid>> RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>> UpdatePermissionsAsync(Guid principalId, IEnumerable<TypedPermission> typedPermissions, CancellationToken cancellationToken = default);
}
