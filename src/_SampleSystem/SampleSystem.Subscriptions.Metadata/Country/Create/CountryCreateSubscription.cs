using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public class CountryCreateSubscription : Subscription<Domain.Directories.Country, _Country_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
