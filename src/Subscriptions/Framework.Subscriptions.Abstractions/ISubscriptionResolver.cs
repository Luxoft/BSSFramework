using System.Collections.Immutable;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public interface ISubscriptionResolver
{
    ImmutableHashSet<Type> DomainTypes { get; }

    ImmutableArray<ISubscription> Resolve(Type domainType, DomainObjectChangeType domainObjectChangeType);
}
