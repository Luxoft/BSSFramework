using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.Events;

public class AuthorizationEventsSubscriptionManager : EventsSubscriptionManager<PersistentDomainObjectBase>
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
