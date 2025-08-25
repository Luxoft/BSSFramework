using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using SecuritySystem;

namespace Framework.Authorization.Notification;

public class NotificationBasePermissionFilterSource(
    IAvailablePermissionSource availablePermissionSource,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver)
    : INotificationBasePermissionFilterSource
{
    public Expression<Func<Permission, bool>> GetBasePermissionFilter(SecurityRole[] securityRoles)
    {
        if (securityRoles == null) throw new ArgumentNullException(nameof(securityRoles));

        var businessRoleIdents = securityRolesIdentsResolver.Resolve(DomainSecurityRule.ExpandedRolesSecurityRule.Create(securityRoles))
                                                            .ToList();

        var permissionQ = availablePermissionSource.GetAvailablePermissionsQueryable(
            DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() });

        return permission => businessRoleIdents.Contains(permission.Role.Id) && permissionQ.Contains(permission);
    }
}
