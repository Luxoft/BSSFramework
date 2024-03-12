using Framework.Events;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManager<PersistentDomainObjectBase>
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
