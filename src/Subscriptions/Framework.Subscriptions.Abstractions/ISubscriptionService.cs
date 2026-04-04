using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionService
{
    IEnumerable<ITryResult<ISubscription>> Process(IDomainObjectVersions versions);
}
