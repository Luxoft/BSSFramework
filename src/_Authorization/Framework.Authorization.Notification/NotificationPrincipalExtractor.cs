using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor(INotificationPermissionExtractor notificationPermissionExtractor) : INotificationPrincipalExtractor
{
    public async Task<IEnumerable<Principal>> GetPrincipalsAsync(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups,
        CancellationToken cancellationToken)
    {
        var permissions = await notificationPermissionExtractor.GetPermissionsAsync(securityRoles, notificationFilterGroups, cancellationToken);

        return permissions.Select(permission => permission.Principal).Distinct();
    }
}
