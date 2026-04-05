using CommonFramework;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class RootSubscriptionService(IServiceProxyFactory serviceProxyFactory, IEnumerable<ISubscriptionMetadata> subscriptionMetadataList) : IRootSubscriptionService
{
    private readonly IReadOnlyDictionary<(Type, DomainObjectChangeType), IRootSubscriptionService> cache =
        subscriptionMetadataList
            .GroupBy(sm => (sm.DomainObjectType, sm.GetConditionLambda()!.DomainObjectChangeType))
            .Select(pair => (pair.Key, serviceProxyFactory.Create<IRootSubscriptionService>(typeof(RootSubscriptionService<>).MakeGenericType(pair.Key.DomainObjectType), pair)))
            .ToDictionary();

    public IEnumerable<ITryResult<ISubscription>> Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.cache.TryGetValue((versions.DomainObjectType, versions.ChangeType), out var innerService) ? innerService.Process(serviceProvider, versions) : [];
}

public class RootSubscriptionService<TDomainObject>( IEnumerable<SubscriptionMetadata<TDomainObject>> subscriptionMetadataList) : IRootSubscriptionService
    where TDomainObject : class
{
    public IEnumerable<ITryResult<ISubscription>> Process(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions)
    {
        throw new NotImplementedException();
    }

    IEnumerable<ITryResult<ISubscription>> IRootSubscriptionService.Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.Process(serviceProvider, (DomainObjectVersions<TDomainObject>)versions);
}

public class SubscriptionService<TDomainObject>(IEnumerable<SubscriptionMetadata<TDomainObject>> subscriptionMetadataList) : IRootSubscriptionService
    where TDomainObject : class
{
    public IEnumerable<ITryResult<ISubscription>> Process(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions)
    {
        throw new NotImplementedException();
    }

    IEnumerable<ITryResult<ISubscription>> IRootSubscriptionService.Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.Process(serviceProvider, (DomainObjectVersions<TDomainObject>)versions);
}
