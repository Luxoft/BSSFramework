using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystemBase(
    IAvailablePermissionSource availablePermissionSource,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    bool withRunAs)
    : IAuthorizationSystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule, withRunAs: withRunAs).Any();
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
