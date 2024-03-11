namespace Framework.Events.DTOMapper;

public interface IDomainEventDTOMapper<in TPersistentDomainObjectBase>
{
    object Convert<TDomainObject>(TDomainObject domainObject, DomainObjectEvent domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase;
}
