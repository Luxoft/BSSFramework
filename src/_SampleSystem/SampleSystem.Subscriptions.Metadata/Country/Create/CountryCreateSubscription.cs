using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

public class CountryCreateSubscription : ISubscription<Domain.Country>
{
    public IEnumerable<NotificationMessageGenerationInfo> GetTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions.Current, versions.Previous);
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetCopyTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions.Current, versions.Previous);
    }
}
