using Framework.Core;
using Framework.Core.Rendering;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class SubscriptionService(IEnumerable<ISubscriptionMetadata> subscriptionMetadataList) : ISubscriptionService
{
    public IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IDomainObjectVersions versions)
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }

    private IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process<TDomainObject>(DomainObjectVersions<TDomainObject> versions)
        where TDomainObject : class
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }
}

public class SubscriptionService<TDomainObject, TSubscription, TMessageTemplate, TRenderingObject>(
    SubscriptionMetadata<TDomainObject, TSubscription, TMessageTemplate> subscriptionMetadata,
    IDomainObjectConverter<TDomainObject, TRenderingObject> renderingObjectConverter,
    TSubscription subscription)
    where TDomainObject : class
    where TSubscription : ISubscription<TDomainObject>
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    public IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.IsProcessed(versions))
        {
            var to = subscription.GetTo(versions);
            var copyTo = subscription.GetCopyTo(versions);
            var replyTo = subscription.GetReplyTo(versions);

            return [];
        }
    }
}


public record SubscriptionFullInfo<TDomainObject>(
    ISubscriptionMetadata SubscriptionMetadata,
    ISubscription<TDomainObject> Subscription,
    IMessageTemplate<TDomainObject> MessageTemplate)
    where TDomainObject : class;
