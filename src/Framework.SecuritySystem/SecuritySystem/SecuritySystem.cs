using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class SecuritySystem(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IReadOnlyList<IPermissionSystem> permissionSystems) : ISecuritySystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return permissionSystems.Any(v => v.GetPermissionSource(securityRule).HasAccess());
    }

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (!this.HasAccess(securityRule))
        {
            throw accessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult { SecurityRule = securityRule });
        }
    }
}
