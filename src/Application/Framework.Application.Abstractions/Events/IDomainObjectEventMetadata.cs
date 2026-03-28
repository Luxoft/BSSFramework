namespace Framework.Application.Events;

public interface IDomainObjectEventMetadata
{
    IEnumerable<EventOperation> GetEventOperations(Type domainType);
}
