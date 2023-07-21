using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups);

    IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups);
}
