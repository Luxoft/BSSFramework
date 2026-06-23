using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionService
{
    Task<List<ITryResult<SubscriptionHeader>>> ProcessAsync(DomainObjectVersions versions, CancellationToken ct);
}
