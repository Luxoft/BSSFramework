using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public interface IPrincipalManagementService : IPrincipalSourceService
{
    Task<IIdentityObject<Guid>> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default);

    Task<IIdentityObject<Guid>> UpdatePrincipalNameAsync(UserCredential userCredential, string principalName, CancellationToken cancellationToken);

    Task<IIdentityObject<Guid>> RemovePrincipalAsync(UserCredential userCredential, bool force, CancellationToken cancellationToken = default);

    Task<MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>> UpdatePermissionsAsync(Guid principalId, IEnumerable<TypedPermission> typedPermissions, CancellationToken cancellationToken = default);
}
