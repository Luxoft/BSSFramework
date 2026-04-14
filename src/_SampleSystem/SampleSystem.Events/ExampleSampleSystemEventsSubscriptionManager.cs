using Framework.Application.Events;
using Framework.BLL.Events.SubscriptionManager;

using SampleSystem.Domain;
using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Models.Event;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class ExampleSampleSystemEventsSubscriptionManager(
    IEventDTOMessageSender<PersistentDomainObjectBase> messageSender,
    ISampleSystemDTOMappingService mappingService)
    : EventsSubscriptionManager<PersistentDomainObjectBase>(messageSender)
{
    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<BusinessUnit>();
        this.SubscribeForSaveOperation<Employee>();
        this.SubscribeForSaveAndRemoveOperation<Information>();

        this.Subscribe<Employee>(
            (_, operation) => operation == EventOperation.Save,
            (domainObject, _) => new EmployeeCustomEventModelSaveEventDTO(mappingService, new EmployeeCustomEventModel(domainObject)));
    }
}
