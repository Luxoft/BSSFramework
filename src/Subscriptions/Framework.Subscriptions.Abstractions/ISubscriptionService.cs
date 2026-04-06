using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionService
{
    IEnumerable<ITryResult<SubscriptionHeader>> Process(IDomainObjectVersions versions);
}
