using Framework.BLL.DTOMapping.DTOMapper;
using Framework.Configuration.BLL;
using Framework.Database.Audit;
using Framework.Infrastructure.LocalDBEvents;

using SampleSystem.Domain;

namespace SampleSystem.Events;

public class SampleSystemCustomAribaLocalDBEventMessageSender(
    IDomainEventDTOMapper<PersistentDomainObjectBase> mapper,
    IConfigurationBLLContext configurationContext,
    IRevisionService revisionService)
    : LocalDBEventMessageSender<PersistentDomainObjectBase>(
        mapper,
        configurationContext,
        revisionService,
        new LocalDBEventMessageSenderSettings<PersistentDomainObjectBase>() { QueueTag = "ariba" });
