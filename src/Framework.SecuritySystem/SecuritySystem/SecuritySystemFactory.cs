using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class SecuritySystemFactory(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IEnumerable<IPermissionSystemFactory> permissionSystems) : ISecuritySystemFactory
{
    public ISecuritySystem Create(SecurityRuleCredential securityRuleCredential)
    {
        return new SecuritySystem(accessDeniedExceptionService, permissionSystems.Select(f => f.Create(securityRuleCredential)).ToList());
    }
}
