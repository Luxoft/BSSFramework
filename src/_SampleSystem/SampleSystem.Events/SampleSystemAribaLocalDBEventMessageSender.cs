using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class SampleSystemAribaLocalDBEventMessageSender : LocalDBEventMessageSender<PersistentDomainObjectBase, EventDTOBase>
{
    private readonly ISampleSystemDTOMappingService mappingService;

    public SampleSystemAribaLocalDBEventMessageSender(
        ISampleSystemDTOMappingService mappingService,
        IConfigurationBLLContext configurationContext)
        : base(configurationContext, "ariba")
    {
        this.mappingService = mappingService;
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject, TOperation>(
        IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
    {
        return DomainEventDTOMapper<TDomainObject, TOperation>.MapToEventDTO(
            this.mappingService,
            domainObjectEventArgs.DomainObject,
            domainObjectEventArgs.Operation);
    }
}
