using Framework.Application.Events;

namespace Framework.BLL.DTOMapping.DTOMapper;

public interface IDomainEventDTOMapper<in TPersistentDomainObjectBase>
{
    object Convert<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase;
}
