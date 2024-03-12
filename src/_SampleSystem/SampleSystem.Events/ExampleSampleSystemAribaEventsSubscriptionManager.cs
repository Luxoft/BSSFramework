using Framework.Events.Legacy;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class ExampleSampleSystemAribaEventsSubscriptionManager : EventsSubscriptionManager<PersistentDomainObjectBase>
{
    public ExampleSampleSystemAribaEventsSubscriptionManager(SampleSystemCustomAribaLocalDBEventMessageSender messageSender)
            : base(messageSender)
    {
    }

    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<Employee>();
    }
}
