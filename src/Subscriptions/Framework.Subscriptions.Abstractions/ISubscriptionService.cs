using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface IRootSubscriptionService
{
    IEnumerable<ITryResult<ISubscription>> Process(IServiceProvider serviceProvider, IDomainObjectVersions versions);
}
