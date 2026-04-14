using Framework.BLL.Events.SubscriptionManager;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

namespace SampleSystem.Events;

public class ExampleSampleSystemAribaEventsSubscriptionManager(SampleSystemCustomAribaLocalDBEventMessageSender messageSender)
    : EventsSubscriptionManager<PersistentDomainObjectBase>(messageSender)
{
    public override void Subscribe() => this.SubscribeForSaveOperation<Employee>();
}
