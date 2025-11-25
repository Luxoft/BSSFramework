using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationPermissionExtractor
{
    Task<List<Permission>> GetPermissionsAsync(SecurityRole[] securityRoles, IEnumerable<NotificationFilterGroup> notificationFilterGroups, CancellationToken cancellationToken = default);
}
