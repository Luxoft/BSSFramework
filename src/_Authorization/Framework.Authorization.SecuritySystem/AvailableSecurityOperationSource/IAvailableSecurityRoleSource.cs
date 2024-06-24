using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityRoleSource
{
    Task<List<FullSecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default);
}
