using CommonFramework;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.Configuration.BLL;

public interface ISystemConstantInitializer : IInitializer;

public interface ISubscriptionBLL
{
    IList<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

    IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(object previous, object current, Type type);

    SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object previous, object current, string subscriptionCode);

    bool HasActiveSubscriptions(Type type);

    bool HasActiveSubscriptions(DomainType domainType);
}
