using Framework.Events;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManagerBase<PersistentDomainObjectBase>
{
    public SampleSystemAribaEventsSubscriptionManager(SampleSystemAribaLocalDBEventMessageSender messageSender)
            : base(messageSender)
    {
    }

    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<Employee>();
    }
}
