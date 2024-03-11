namespace Framework.Events;

public class DomainObjectEventMetadata : IDomainObjectEventMetadata
{
    public virtual IEnumerable<DomainObjectEvent> GetEventOperations(Type domainType)
    {
        yield return DomainObjectEvent.Save;
        yield return DomainObjectEvent.Remove;
    }
}
