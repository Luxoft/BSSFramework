using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database.Domain;

namespace Framework.Configuration.BLL;

public interface ISubscriptionBLL
{
    List<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

    List<ITryResult<Subscription>> ProcessChangedObjectUntyped(object previous, object current, Type type);

    SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object previous, object current, string subscriptionCode);

    bool HasActiveSubscriptions(Type type);

    bool HasActiveSubscriptions(DomainType domainType);
}
