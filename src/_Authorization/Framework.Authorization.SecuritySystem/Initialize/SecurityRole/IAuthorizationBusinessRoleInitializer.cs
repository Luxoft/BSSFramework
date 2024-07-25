using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Initialize;

public interface IAuthorizationBusinessRoleInitializer : ISecurityInitializer
{
    Task<MergeResult<BusinessRole, FullSecurityRole>> Init(
        IEnumerable<FullSecurityRole> securityRoles,
        CancellationToken cancellationToken = default);

    new Task<MergeResult<BusinessRole, FullSecurityRole>> Init(CancellationToken cancellationToken = default);
}
