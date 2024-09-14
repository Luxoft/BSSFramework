using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class SecuritySystem(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IEnumerable<IPermissionSystem> permissionSystems,
    bool withRunAs) : ISecuritySystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return permissionSystems.Any(v => v.GetPermissionSource(securityRule, withRunAs).HasAccess());
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
