using Framework.Core;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SecuritySystem.Notification;

namespace Framework.Subscriptions;

public class SubscriptionService(IEnumerable<ISubscription> subscriptionMetadataList) : ISubscriptionService
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(IDomainObjectVersions versions)
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }

    private IEnumerable<ITryResult<SubscriptionHeader>> Process<TDomainObject>(DomainObjectVersions<TDomainObject> versions)
        where TDomainObject : class
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }
}

public class SubscriptionService<TDomainObject, TRenderingObject, TPrincipal>(
    IServiceProvider serviceProvider,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    INotificationPrincipalExtractor<TPrincipal> notificationPrincipalExtractor)
    where TDomainObject : class
    where TRenderingObject : class
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.IsProcessed(serviceProvider, versions))
        {
            var to = subscription.GetTo(serviceProvider, versions);
            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);



            yield return TryResult.Return(subscription.Header);
        }
    }
}


//public record SubscriptionFullInfo<TDomainObject, TRenderingObject>(
//    ISubscriptionMetadata Metadata,
//    ISubscription<TDomainObject, TRenderingObject> Subscription,
//    IMessageTemplate<TRenderingObject> MessageTemplate)
//    where TDomainObject : class
//    where TRenderingObject : class;
