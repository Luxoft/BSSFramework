using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events.Legacy;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemCustomAribaLocalDBEventMessageSender(
    IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
    IConfigurationBLLContext configurationContext)
    : LocalDBEventMessageSender<PersistentDomainObjectBase>(
        mapper,
        configurationContext,
        new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" });
