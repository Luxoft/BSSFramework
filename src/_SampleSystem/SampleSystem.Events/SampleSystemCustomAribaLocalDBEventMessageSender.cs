using Framework.BLL.DTOMapping.DTOMapper;
using Framework.Configuration.BLL;
using Framework.Database;
using Framework.Infrastructure.LocalDBEvents;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemCustomAribaLocalDBEventMessageSender(
    IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
    IConfigurationBLLContext configurationContext,
    ICurrentRevisionService currentRevisionService)
    : LocalDBEventMessageSender<PersistentDomainObjectBase>(
        mapper,
        configurationContext,
        currentRevisionService,
        new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" });
