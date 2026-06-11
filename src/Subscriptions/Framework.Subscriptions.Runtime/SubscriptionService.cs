using Anch.Core;

using Framework.Core;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class SubscriptionService(
    IServiceProxyFactory serviceProxyFactory,
    ISubscriptionResolver subscriptionResolver,
    IMessageSender<Notification.Domain.Notification> notificationMessageSender) : ISubscriptionService
{
    public IAsyncEnumerable<ITryResult<SubscriptionHeader>> ProcessAsync(DomainObjectVersions versions) =>
        subscriptionResolver.Resolve(versions.DomainObjectType, versions.ChangeType)
                            .ToAsyncEnumerable()
                            .Select(async (ISubscription subscription, CancellationToken ct) =>
                            {
                                try
                                {
                                    await new Func<ISubscription<object, object>, DomainObjectVersions<object>, CancellationToken, Task>(this.ProcessAsync)
                                          .CreateGenericMethod(subscription.DomainObjectType, subscription.RenderingObjectType)
                                          .Invoke<Task>(this, subscription, versions, ct);

                                    return TryResult.Return(subscription.Header);
                                }
                                catch (Exception ex)
                                {
                                    return TryResult.CreateFault<SubscriptionHeader>(ex);
                                }
                            });

    private async Task ProcessAsync<TDomainObject, TRenderingObject>(
        ISubscription<TDomainObject, TRenderingObject> subscription,
        DomainObjectVersions<TDomainObject> versions,
        CancellationToken ct)
        where TDomainObject : class
        where TRenderingObject : class
    {
        var notificationExtractor = serviceProxyFactory.Create<NotificationExtractor<TDomainObject, TRenderingObject>>(subscription);

        await foreach (var notification in notificationExtractor.GetNotifications(versions).WithCancellation(ct))
        {
            await notificationMessageSender.SendAsync(notification, ct);
        }
    }
}
