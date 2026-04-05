using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator;

public class AttachmentTemplateEvaluatorSubscriptionMetadata
    : SubscriptionMetadata<Domain.Employee, AttachmentTemplateEvaluatorSubscription, _Examples_AttachmentTemplateEvaluator_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderName { get; } = "SampleSystem";

    public override string SenderEmail { get; } = "AttachmentTemplateEvaluator@luxoft.com";

    public override bool IncludeAttachments { get; } = true;
}
