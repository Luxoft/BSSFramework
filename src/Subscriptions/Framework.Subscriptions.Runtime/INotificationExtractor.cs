using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface INotificationExtractor<TDomainObject>
    where TDomainObject : class
{
    IAsyncEnumerable<Notification.Domain.Notification> GetNotifications(DomainObjectVersions<TDomainObject> versions);
}
