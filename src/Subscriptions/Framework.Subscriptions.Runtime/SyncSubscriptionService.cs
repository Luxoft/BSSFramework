using Anch.Core;

using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public class SyncSubscriptionService(ISubscriptionService subscriptionService, IDefaultCancellationTokenSource? defaultCancellationTokenSource = null)
    : ISyncSubscriptionService
{
    public List<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions versions) =>
        defaultCancellationTokenSource.RunSync(async ct => await subscriptionService.ProcessAsync(versions, ct));
}
