using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystemBase(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IAvailablePermissionSource availablePermissionSource,
    bool withRunAs)
    : SecuritySystemBase(accessDeniedExceptionService)
{
    public override bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule, withRunAs: withRunAs).Any();
    }
}
