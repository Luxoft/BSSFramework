using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class SecuritySystemFactory(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IEnumerable<IPermissionSystem> permissionSystems) : ISecuritySystemFactory
{
    public ISecuritySystem Create(bool withRunAs)
    {
        return new SecuritySystem(accessDeniedExceptionService, permissionSystems, withRunAs);
    }
}
