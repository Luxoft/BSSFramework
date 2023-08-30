using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public class SampleSystemLocalDBEventMessageSender : LocalDBEventMessageSender<ISampleSystemBLLContext, PersistentDomainObjectBase, EventDTOBase>
{
    public SampleSystemLocalDBEventMessageSender(ISampleSystemBLLContext context, IConfigurationBLLContext configurationContext)
            : base(context, configurationContext)
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
