using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups);
}
