using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class RootAuthorizationSystem(IEnumerable<IPermissionSystem> authorizationSystems) : IAuthorizationSystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return authorizationSystems.Any(v => v.HasAccess(securityRule));
    }

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        foreach (var authorizationSystem in authorizationSystems)
        {
            authorizationSystem.CheckAccess(securityRule);
        }
    }
}
