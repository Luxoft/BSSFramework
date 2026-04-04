using Framework.Subscriptions.Metadata;

using SampleSystem.Subscriptions.Metadata.Employee.Update;
using SampleSystem.Subscriptions.Metadata.Examples.Attachment;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentInline;

public sealed class AttachmentInlineSubscription
        : SubscriptionMetadata<Domain.Employee, _Examples_AttachmentInline_MessageTemplate_cshtml>
{
    /// <summary>
    /// Sample with inline attachment
    /// </summary>
    public AttachmentInlineSubscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "InlineAttach@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = [new SecurityItemSourceLambda()];
        this.RecipientsSelectorMode = RecipientsSelectorMode.Union;
        this.IncludeAttachments = true;
        this.AttachmentLambda = new AttachmentLambda();
    }
}
