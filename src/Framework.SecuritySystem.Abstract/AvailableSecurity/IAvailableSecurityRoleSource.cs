namespace Framework.SecuritySystem.AvailableSecurity;

public interface IAvailableSecurityRoleSource
{
    Task<List<FullSecurityRole>> GetAvailableSecurityRoles(bool expandChildren = true, CancellationToken cancellationToken = default);
}
