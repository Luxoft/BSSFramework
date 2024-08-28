using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionsSystem(
    IAvailablePermissionSource availablePermissionSource,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IServiceProvider serviceProvider)
    : AuthorizationSystemBase(availablePermissionSource, accessDeniedExceptionService, true), IPermissionSystem
{
    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationPermissionSource>(serviceProvider, securityRule);
    }
}
