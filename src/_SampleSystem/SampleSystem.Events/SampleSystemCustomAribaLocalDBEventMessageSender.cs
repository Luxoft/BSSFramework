using Framework.BLL.DTOMapping.DTOMapper;
using Framework.Infrastructure.LocalDBEvents;

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
