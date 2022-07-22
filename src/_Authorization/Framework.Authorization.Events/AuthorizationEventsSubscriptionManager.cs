using System;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events
{
    public class AuthorizationEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
    {
        public AuthorizationEventsSubscriptionManager(IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
            : base(messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Principal>();

            this.SubscribeForSaveAndRemoveOperation<PermissionFilterItem>();
        }
    }
}
