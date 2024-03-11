using Framework.Events;

namespace SampleSystem.Domain;

public class SampleSystemEventOperationSource : DomainObjectEventMetadata
{
    public override IEnumerable<DomainObjectEvent> GetEventOperations(Type domainType)
    {
        if (domainType == typeof(Employee))
        {
            return [DomainObjectEvent.Save];
        }
        else
        {
            return base.GetEventOperations(domainType);
        }
    }
}
