using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

namespace Framework.Authorization.Notification;

public class NotificationBasePermissionFilterSource(
    IAvailablePermissionSource availablePermissionSource,
    ISecurityRoleSource securityRoleSource)
    : INotificationBasePermissionFilterSource
{
    public Expression<Func<Permission, bool>> GetBasePermissionFilter(SecurityRole[] securityRoles)
    {
        if (securityRoles == null) throw new ArgumentNullException(nameof(securityRoles));

        var businessRoleIdents = securityRoles.Select(sr => securityRoleSource.GetFullRole(sr).Id).ToList();

        var permissionQ = availablePermissionSource.GetAvailablePermissionsQueryable(applyCurrentUser: false);

        return permission => businessRoleIdents.Contains(permission.Role.Id) && permissionQ.Contains(permission);
    }
}
