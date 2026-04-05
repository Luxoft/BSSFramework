using ASP;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class SubscriptionMetadata : SubscriptionMetadata<Domain.Country, Subscription, _DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderEmail { get; } = "SampleSystem";

    public override string SenderName { get; } = "SampleSystem@luxoft.com";

    public override bool AllowEmptyListOfRecipients { get; } = false;

    public override bool IncludeAttachments { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool SendIndividualLetters { get; } = true;
}
