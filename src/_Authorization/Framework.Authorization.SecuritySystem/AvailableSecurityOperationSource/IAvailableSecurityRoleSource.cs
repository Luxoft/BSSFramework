using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityRoleSource
{
    Task<List<FullSecurityRole>> GetAvailableSecurityRole(CancellationToken cancellationToken = default);
}
