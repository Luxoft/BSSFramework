using Framework.Events;
using Framework.Events.Legacy;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class ExampleSampleSystemEventsSubscriptionManager(
    IEventDTOMessageSender<PersistentDomainObjectBase> messageSender,
    ISampleSystemDTOMappingService mappingService)
    : EventsSubscriptionManager<PersistentDomainObjectBase>(messageSender)
{
    private readonly ISampleSystemDTOMappingService mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));

    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<BusinessUnit>();
        this.SubscribeForSaveOperation<Employee>();
        this.SubscribeForSaveAndRemoveOperation<Information>();

        this.Subscribe<Employee>(
            (_, operation) => operation == EventOperation.Save,
            (domainObject, _) => new EmployeeCustomEventModelSaveEventDTO(this.mappingService, new EmployeeCustomEventModel(domainObject)));
    }
}
