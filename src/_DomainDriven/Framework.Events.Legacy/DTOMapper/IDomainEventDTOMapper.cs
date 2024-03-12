namespace Framework.Events.Legacy;

public interface IDomainEventDTOMapper<in TPersistentDomainObjectBase>
{
    object Convert<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase;
}
