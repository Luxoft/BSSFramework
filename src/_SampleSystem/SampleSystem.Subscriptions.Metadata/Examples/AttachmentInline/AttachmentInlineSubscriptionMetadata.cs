using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentInline;

public class AttachmentInlineSubscriptionMetadata
    : SubscriptionMetadata<Domain.Employee, AttachmentInlineSubscription, _Examples_AttachmentInline_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "InlineAttach@luxoft.com";

    public override bool IncludeAttachments { get; } = true;
}
