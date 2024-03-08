﻿using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class SampleSystemLocalDBEventMessageSender : LocalDBEventMessageSender<PersistentDomainObjectBase, EventDTOBase>
{
    private readonly ISampleSystemDTOMappingService mappingService;

    public SampleSystemLocalDBEventMessageSender(ISampleSystemDTOMappingService mappingService, IConfigurationBLLContext configurationContext)
            : base(configurationContext)
    {
        this.mappingService = mappingService;
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
    {
        return DomainEventDTOMapper<TDomainObject>.MapToEventDTO(
            this.mappingService,
            domainObjectEventArgs.DomainObject,
            domainObjectEventArgs.Operation);
    }
}
