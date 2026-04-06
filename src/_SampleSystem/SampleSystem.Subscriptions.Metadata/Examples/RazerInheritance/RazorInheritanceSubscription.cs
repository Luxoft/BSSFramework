using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance;

public class RazorInheritanceSubscription : ISubscription<Domain.Employee>
{
    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
