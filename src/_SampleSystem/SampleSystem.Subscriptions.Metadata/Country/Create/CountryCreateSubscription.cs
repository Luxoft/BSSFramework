using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

public class CountryCreateSubscription : ISubscription<Domain.Country>
{
    public IEnumerable<NotificationMessageGenerationInfo<Domain.Country>> GetTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Country>> GetCopyTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
