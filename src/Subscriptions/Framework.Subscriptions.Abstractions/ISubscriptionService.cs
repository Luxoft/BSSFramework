using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionService
{
    IAsyncEnumerable<ITryResult<SubscriptionHeader>> ProcessAsync(DomainObjectVersions versions);
}
