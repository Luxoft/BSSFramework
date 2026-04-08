using CommonFramework;

using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public class SubscriptionService(
    IServiceProxyFactory serviceProxyFactory,
    ISubscriptionResolver subscriptionResolver,
    IMessageSender<Notification.Domain.Notification> notificationMessageSender,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource) : ISubscriptionService
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions versions)
    {
        foreach (var subscription in subscriptionResolver.Resolve(versions.DomainObjectType, versions.ChangeType))
        {
            yield return TryResult.Catch(() =>
            {
                var notificationExtractorType = typeof(NotificationExtractor<,>).MakeGenericType(subscription.DomainObjectType, subscription.RenderingObjectType);

                var notificationExtractor = serviceProxyFactory.Create<INotificationExtractor>(notificationExtractorType, subscription);

                foreach (var notification in notificationExtractor.GetNotifications(versions))
                {
                    defaultCancellationTokenSource.RunSync(async ct => await notificationMessageSender.SendAsync(notification, ct));
                }

                return subscription.Header;
            });
        }
    }
}
