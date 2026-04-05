using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

public class RazorTemplateImplSubscription : ISubscription<Domain.Employee>
{
    public IEnumerable<NotificationMessageGenerationInfo> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new ("tester@luxoft.com", versions.Previous, versions.Current);
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new ("tester@luxoft.com", versions.Previous, versions.Current);
    }
}
