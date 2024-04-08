using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityRoleSource
{
    Task<List<SecurityRole>> GetAvailableSecurityRole(CancellationToken cancellationToken = default);
}
