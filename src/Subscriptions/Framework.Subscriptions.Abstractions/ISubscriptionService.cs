using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface ISubscriptionService
{
    IEnumerable<ITryResult<ISubscriptionMetadataBase>> Process(IDomainObjectVersions versions);
}
