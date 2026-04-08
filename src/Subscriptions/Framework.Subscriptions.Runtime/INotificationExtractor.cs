using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface INotificationExtractor
{
    IEnumerable<Notification.Domain.Notification> GetNotifications(DomainObjectVersions versions);
}
