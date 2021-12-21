using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

namespace Framework.Authorization.Events
{
    public class AuthorizationEventsSubscriptionManager : EventsSubscriptionManagerBase<IAuthorizationBLLContext, PersistentDomainObjectBase>
    {
        public AuthorizationEventsSubscriptionManager(IAuthorizationBLLContext context, IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender)
            : base(context, messageSender)
        {
        }

        public override void Subscribe()
        {
            this.SubscribeForSaveOperation<Principal>();

            this.SubscribeForSaveAndRemoveOperation<PermissionFilterItem>();
        }
    }
}
