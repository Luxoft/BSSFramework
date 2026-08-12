using System.Collections.Immutable;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class SubscriptionResolver(IEnumerable<ISubscription> subscriptions) : ISubscriptionResolver
{
    private readonly IReadOnlyDictionary<(Type, DomainObjectChangeType), ImmutableArray<ISubscription>> cache =
        subscriptions
            .SelectMany(sm => new[] { DomainObjectChangeType.Create, DomainObjectChangeType.Update, DomainObjectChangeType.Delete }
                              .Where(changeType => sm.DomainObjectChangeType.HasFlag(changeType))
                              .Select(changeType => (sm, changeType)))

            .GroupBy(pair => (pair.sm.DomainObjectType, pair.changeType), pair => pair.sm)
            .ToDictionary(pair => pair.Key, pair => pair.ToImmutableArray());

    public ImmutableHashSet<Type> DomainTypes => field ??= [.. this.cache.Keys.Select(pair => pair.Item1)];

    public ImmutableArray<ISubscription> Resolve(Type domainType, DomainObjectChangeType domainObjectChangeType) =>
        this.cache.GetValueOrDefault((domainType, domainObjectChangeType), []);
}
