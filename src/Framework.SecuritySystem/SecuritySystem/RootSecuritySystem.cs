using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class RootSecuritySystem(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IEnumerable<IPermissionSystem> permissionSystems) : SecuritySystemBase(accessDeniedExceptionService)
{
    public override bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return permissionSystems.Any(v => v.HasAccess(securityRule));
    }
}
