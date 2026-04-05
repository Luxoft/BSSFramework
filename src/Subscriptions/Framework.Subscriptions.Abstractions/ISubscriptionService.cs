using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface IRootSubscriptionService
{
    IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IServiceProvider serviceProvider, IDomainObjectVersions versions);
}
