using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionMetadataService
{
    IEnumerable<ITryResult<SubscriptionHeader>> Process(Type domainType, DomainObjectChangeType domainObjectChangeType);
}
