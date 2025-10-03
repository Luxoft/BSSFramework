using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    IEnumerable<Principal> GetNotificationPrincipalsByRoles(SecurityRole[] securityRoles, IEnumerable<NotificationFilterGroup> notificationFilterGroups);
}
