using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityRoleSource
{
    Task<List<FullSecurityRole>> GetAvailableSecurityRoles(bool expandChildren = true, CancellationToken cancellationToken = default);
}
