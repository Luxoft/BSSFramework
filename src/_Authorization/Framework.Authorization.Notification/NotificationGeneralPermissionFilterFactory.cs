using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;

using SecuritySystem;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public class NotificationGeneralPermissionFilterFactory(
    IAvailablePermissionSource availablePermissionSource,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver)
    : INotificationGeneralPermissionFilterFactory
{
    public Expression<Func<Permission, bool>> Create(IEnumerable<SecurityRole> securityRoles)
    {
        var businessRoleIdents = securityRolesIdentsResolver.Resolve(DomainSecurityRule.ExpandedRolesSecurityRule.Create(securityRoles))
                                                            .ToHashSet();

        var permissionQ = availablePermissionSource.GetAvailablePermissionsQueryable(
            DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() });

        return permission => businessRoleIdents.Contains(permission.Role.Id) && permissionQ.Contains(permission);
    }
}
