using System.Linq.Expressions;

using Framework.Authorization.Domain;

using SecuritySystem;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public class NotificationGeneralPermissionFilterFactory(
    IAvailablePermissionFilterFactory<Permission> availablePermissionFilterFactory)
    : INotificationGeneralPermissionFilterFactory
{
    public Expression<Func<Permission, bool>> Create(IEnumerable<SecurityRole> securityRoles) =>
        availablePermissionFilterFactory.CreateFilter(
            DomainSecurityRule.ExpandedRolesSecurityRule.Create(securityRoles) with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() });
}
