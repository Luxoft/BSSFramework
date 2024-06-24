using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Initialize;

public interface IAuthorizationBusinessRoleInitializer : ISecurityInitializer
{
    Task Init(IEnumerable<FullSecurityRole> securityRoles, CancellationToken cancellationToken = default);
}
