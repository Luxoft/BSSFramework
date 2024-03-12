namespace Framework.Events;

public interface IDomainObjectEventMetadata
{
    IEnumerable<EventOperation> GetEventOperations(Type domainType);
}
