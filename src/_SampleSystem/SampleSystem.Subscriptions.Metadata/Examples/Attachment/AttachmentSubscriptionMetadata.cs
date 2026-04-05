using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public class AttachmentSubscriptionMetadata
    : SubscriptionMetadata<Domain.Employee, AttachmentSubscription, _Examples_Attachment_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "Attachment@luxoft.com";

    public override bool IncludeAttachments { get; } = true;
}
