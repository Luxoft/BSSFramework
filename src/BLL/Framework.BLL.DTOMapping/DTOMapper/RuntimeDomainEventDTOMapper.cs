using Framework.Application.Events;

namespace Framework.BLL.DTOMapping.DTOMapper;

public class RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase, TMappingService, TEventDTOBase>(
    TMappingService mappingService,
    RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase> converter)
    : IDomainEventDTOMapper<TPersistentDomainObjectBase>
{
    public virtual object Convert<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase =>
        converter.Convert(mappingService, domainObject, domainObjectEvent);
}
