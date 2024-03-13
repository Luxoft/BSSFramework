using Framework.Events;

namespace SampleSystem.Domain;

public class SampleSystemDomainObjectEventMetadata : DomainObjectEventMetadata
{
    public override IEnumerable<EventOperation> GetEventOperations(Type domainType)
    {
        if (domainType == typeof(Employee))
        {
            return [EventOperation.Save];
        }
        else
        {
            return base.GetEventOperations(domainType);
        }
    }
}
