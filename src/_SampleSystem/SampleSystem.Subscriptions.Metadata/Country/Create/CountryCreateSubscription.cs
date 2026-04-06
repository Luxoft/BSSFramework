using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public class CountryCreateSubscription : Subscription<Domain.Country, _Country_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "SampleSystem@luxoft.com";

    public override bool SendIndividualLetters { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool IncludeAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = false;

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Country>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Country>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
