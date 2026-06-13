using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISyncSubscriptionService
{
    List<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions versions);
}
