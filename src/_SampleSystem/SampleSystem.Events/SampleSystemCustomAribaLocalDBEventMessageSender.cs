using Framework.BLL.DTOMapping.DTOMapper;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events.Legacy;

using SampleSystem.Domain;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace SampleSystem.Events;

public class SampleSystemCustomAribaLocalDBEventMessageSender(
    IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
    IConfigurationBLLContext configurationContext)
    : LocalDBEventMessageSender<PersistentDomainObjectBase>(
        mapper,
        configurationContext,
        new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" });
