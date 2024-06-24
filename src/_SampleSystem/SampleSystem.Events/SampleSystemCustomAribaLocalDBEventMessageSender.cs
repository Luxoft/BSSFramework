using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events.Legacy;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemCustomAribaLocalDBEventMessageSender : LocalDBEventMessageSender<PersistentDomainObjectBase>
{
    public SampleSystemCustomAribaLocalDBEventMessageSender(
        IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
        IConfigurationBLLContext configurationContext)
        : base(mapper, configurationContext, new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" })
    {
    }
}
