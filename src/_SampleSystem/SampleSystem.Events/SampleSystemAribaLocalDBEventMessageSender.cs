using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events.DTOMapper;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemAribaLocalDBEventMessageSender : LocalDBEventMessageSender<PersistentDomainObjectBase>
{
    public SampleSystemAribaLocalDBEventMessageSender(
        IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
        IConfigurationBLLContext configurationContext)
        : base(mapper, configurationContext, new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" })
    {
    }
}
