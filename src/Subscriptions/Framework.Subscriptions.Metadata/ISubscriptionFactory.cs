using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface ISubscriptionFactory<TDomainObject>
    where TDomainObject : class
{
    ISubscription<TDomainObject> Create(DomainObjectVersions<TDomainObject> versions);
}
