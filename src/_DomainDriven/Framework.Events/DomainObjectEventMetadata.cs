namespace Framework.Events;

public class DomainObjectEventMetadata : IDomainObjectEventMetadata
{
    public virtual IEnumerable<EventOperation> GetEventOperations(Type domainType)
    {
        yield return EventOperation.Save;
        yield return EventOperation.Remove;
    }
}
