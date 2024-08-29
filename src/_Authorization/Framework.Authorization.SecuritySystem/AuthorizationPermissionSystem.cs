using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSystem(
    IAvailablePermissionSource availablePermissionSource,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IServiceProvider serviceProvider)
    : AuthorizationSystemBase(availablePermissionSource, accessDeniedExceptionService, true), IPermissionSystem
{
    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationPermissionSource>(serviceProvider, securityRule);
    }

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationAvailableSecurityRoleSource>(serviceProvider)
                                 .GetAvailableSecurityRoles(cancellationToken);
    }
}
