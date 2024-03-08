namespace Framework.Events;

public class EventOperationSource : IEventOperationSource
{
    public virtual IEnumerable<EventOperation> GetEventOperations(Type domainType)
    {
        yield return EventOperation.Save;
        yield return EventOperation.Remove;
    }
}
