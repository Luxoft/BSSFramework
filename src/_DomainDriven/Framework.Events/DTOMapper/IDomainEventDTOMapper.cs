namespace Framework.Events.DTOMapper;

public interface IDomainEventDTOMapper<in TPersistentDomainObjectBase>
{
    object Convert<TDomainObject>(TDomainObject domainObject, EventOperation eventOperation)
        where TDomainObject : TPersistentDomainObjectBase;
}
