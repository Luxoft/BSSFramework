using Framework.BLL.Events.SubscriptionManager;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class ExampleSampleSystemAribaEventsSubscriptionManager(SampleSystemCustomAribaLocalDBEventMessageSender messageSender)
    : EventsSubscriptionManager<PersistentDomainObjectBase>(messageSender)
{
    public override void Subscribe() => this.SubscribeForSaveOperation<Employee>();
}
