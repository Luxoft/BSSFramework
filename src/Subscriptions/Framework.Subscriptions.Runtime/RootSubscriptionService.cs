using CommonFramework;

using Framework.Core;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class RootSubscriptionService(IServiceProxyFactory serviceProxyFactory, IEnumerable<ISubscriptionMetadata> subscriptionMetadataList) : IRootSubscriptionService
{
    private readonly IReadOnlyDictionary<(Type, DomainObjectChangeType), IRootSubscriptionService> cache =
        subscriptionMetadataList
            .SelectMany(sm => new[] { DomainObjectChangeType.Create, DomainObjectChangeType.Update, DomainObjectChangeType.Delete }
                            .Where(changeType => sm.DomainObjectChangeType.HasFlag(changeType))
                            .Select(changeType => (sm, changeType)))

            .GroupBy(pair => (pair.sm.DomainObjectType, pair.changeType), pair => pair.sm)
            .Select(pair => (pair.Key, serviceProxyFactory.Create<IRootSubscriptionService>(typeof(RootSubscriptionService<,,>).MakeGenericType(pair.Key.DomainObjectType, pair.Key), pair)))
            .ToDictionary();

    public IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.cache.TryGetValue((versions.DomainObjectType, versions.ChangeType), out var innerService) ? innerService.Process(serviceProvider, versions) : [];
}

public class RootSubscriptionService<TDomainObject, TSubscription, TMessageTemplate>(IEnumerable<SubscriptionMetadata<TDomainObject, TSubscription, TMessageTemplate>> subscriptionMetadataList) : IRootSubscriptionService
    where TDomainObject : class
    where TSubscription : ISubscription<TDomainObject>
    where TMessageTemplate : IMessageTemplate
{
    public IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions)
    {
        var filteredByCondition = subscriptionMetadataList.Where(sm => sm.ConditionLambda.Lambda(serviceProvider, versions));


        throw new NotImplementedException();
    }

    IEnumerable<ITryResult<ISubscriptionMetadataBase>> IRootSubscriptionService.Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.Process(serviceProvider, (DomainObjectVersions<TDomainObject>)versions);
}

public class RootSubscriptionService<TDomainObject, TRenderingObject, TMessageTemplate>(SubscriptionMetadata<TDomainObject, TRenderingObject, TMessageTemplate> subscription) : IRootSubscriptionService
    where TDomainObject : class
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    public IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions)
    {
        var toInfoList = subscription.GenerationLambda.Lambda . toProcessor.Invoke(subscription, versions).ToList();
        var ccInfoList = ccProcessor.Invoke(subscription, versions).ToList();
        var replyToInfoList = replyToProcessor.Invoke(subscription, versions).ToList();

        var filteredByCondition = subscriptionMetadataList.Where(sm => sm.ConditionLambda.Lambda(serviceProvider, versions));


        throw new NotImplementedException();
    }

    IEnumerable<ITryResult<ISubscriptionMetadataBase>> IRootSubscriptionService.Process(IServiceProvider serviceProvider, IDomainObjectVersions versions) =>
        this.Process(serviceProvider, (DomainObjectVersions<TDomainObject>)versions);
}
