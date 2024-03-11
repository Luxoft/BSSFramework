namespace Framework.Events;

public interface IDomainObjectEventMetadata
{
    IEnumerable<DomainObjectEvent> GetEventOperations(Type domainType);
}
