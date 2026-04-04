using System.Collections.Concurrent;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class SubscriptionService(IEnumerable<ISubscriptionMetadata> subscriptionMetadataList) : ISubscriptionService
{
    private readonly IReadOnlyDictionary<Type, ISubscriptionMetadata> cache = subscriptionMetadataList
        .GroupBy(sm => sm.DomainObjectType)
        .Select(pair => (pair.Key, pair))

    public IEnumerable<ITryResult<ISubscription>> Process(IDomainObjectVersions versions)
    {
        return
        throw new NotImplementedException();
    }
}

public class SubscriptionService<TDomainObject>(SubscriptionMetadata<>) : ISubscriptionService
    where TDomainObject : class
{
    public IEnumerable<ITryResult<ISubscription>> Process(DomainObjectVersions<TDomainObject> versions)
    {
        throw new NotImplementedException();
    }

    IEnumerable<ITryResult<ISubscription>> ISubscriptionService.Process(IDomainObjectVersions versions) => this.Process((DomainObjectVersions<TDomainObject>)versions);
}
