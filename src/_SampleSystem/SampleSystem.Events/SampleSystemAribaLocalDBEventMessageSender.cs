using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class SampleSystemAribaLocalDBEventMessageSender : LocalDBEventMessageSender<ISampleSystemBLLContext, PersistentDomainObjectBase, EventDTOBase>
{
    public SampleSystemAribaLocalDBEventMessageSender(ISampleSystemBLLContext context, IConfigurationBLLContext configurationContext)
            : base(context, configurationContext, "ariba")
    {
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
    {
        return DomainEventDTOMapper<TDomainObject, TOperation>.MapToEventDTO(
                                                                             new SampleSystemServerPrimitiveDTOMappingService(this.Context),
                                                                             domainObjectEventArgs.DomainObject,
                                                                             domainObjectEventArgs.Operation);
    }
}
