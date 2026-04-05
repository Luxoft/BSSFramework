using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public class CountryCreateSubscriptionMetadata : SubscriptionMetadata<Domain.Country, CountryCreateSubscription, _Country_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "SampleSystem@luxoft.com";

    public override bool SendIndividualLetters { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool IncludeAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = false;
}
