﻿using Framework.Events;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class SampleSystemEventsSubscriptionManager : EventsSubscriptionManager<PersistentDomainObjectBase>
{
    private readonly ISampleSystemDTOMappingService mappingService;

    public SampleSystemEventsSubscriptionManager(
        IEventDTOMessageSender<PersistentDomainObjectBase> messageSender,
            ISampleSystemDTOMappingService mappingService)
            : base(messageSender)
    {
        this.mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
    }

    public override void Subscribe()
    {
        this.SubscribeForSaveOperation<BusinessUnit>();
        this.SubscribeForSaveOperation<Employee>();
        this.SubscribeForSaveAndRemoveOperation<Information>();

        this.SubscribeCustom<Employee>(
            _ => true,
            operation => operation == DomainObjectEvent.Save,
            domainObject => new EmployeeCustomEventModelSaveEventDTO(this.mappingService, new EmployeeCustomEventModel(domainObject)));
    }
}
