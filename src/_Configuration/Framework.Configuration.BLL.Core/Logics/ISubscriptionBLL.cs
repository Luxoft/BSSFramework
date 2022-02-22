using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.Configuration.BLL
{
    public partial interface ISubscriptionBLL
    {
        bool HasActiveSubscriptions(Type type);

        bool HasActiveSubscriptions(DomainType domainType);

        IQueryable<Subscription> GetActiveSubscriptions(DomainType domainType, bool withCondition = true);

        IList<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

        IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(object prev, object next, Type type);

        SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object prev, object next, string subscriptionCode);

        void ValidateAllBusunessRoles();

        void ValidateBusunessRole(Subscription subscription);

        void SynchronizeAllBusunessRoles(bool strong);

        void SynchronizeBusunessRole(Subscription subscription, bool strong);
    }
}
