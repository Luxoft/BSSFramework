using System;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

namespace Framework.Authorization.Events
{
    public class AuthorizationEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
    {
        public AuthorizationEventsSubscriptionManager(IBLLOperationEventListenerContainer<PersistentDomainObjectBase> operationListeners, IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
            : base(operationListeners, messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Principal>();

            this.SubscribeForSaveAndRemoveOperation<PermissionFilterItem>();
        }
    }
}
