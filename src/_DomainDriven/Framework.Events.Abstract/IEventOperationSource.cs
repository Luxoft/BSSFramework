namespace Framework.Events;

public interface IEventOperationSource
{
    IEnumerable<EventOperation> GetEventOperations(Type domainType);
}
