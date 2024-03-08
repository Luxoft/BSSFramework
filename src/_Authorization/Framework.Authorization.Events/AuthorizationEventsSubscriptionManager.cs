using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.Events;

public class AuthorizationEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
{
    public AuthorizationEventsSubscriptionManager(IEventDTOMessageSender<PersistentDomainObjectBase> messageSender)
            : base(messageSender)
    {
    }

    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<Principal>();

        this.SubscribeForSaveAndRemoveOperation<PermissionFilterItem>();
    }
}
