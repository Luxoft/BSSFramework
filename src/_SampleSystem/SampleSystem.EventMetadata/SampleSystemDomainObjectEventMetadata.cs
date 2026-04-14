using Framework.Application.Events;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

namespace SampleSystem.EventMetadata;

public class SampleSystemDomainObjectEventMetadata : DomainObjectEventMetadata
{
    public override IEnumerable<EventOperation> GetEventOperations(Type domainType)
    {
        if (domainType == typeof(Employee))
        {
            return [EventOperation.Save, SampleSystemEventOperation.CustomAction];
        }
        else
        {
            return base.GetEventOperations(domainType);
        }
    }
}
