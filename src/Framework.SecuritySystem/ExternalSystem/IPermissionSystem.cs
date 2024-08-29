namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSystem : IAuthorizationSystem
{
    IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default);
}
