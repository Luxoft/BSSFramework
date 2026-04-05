using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public sealed class AttachmentSubscription
    : SubscriptionMetadata<Domain.Employee, _Examples_Attachment_MessageTemplate_cshtml>
{
    /// <summary>
    /// Sample with inline attachment
    /// </summary>
    public AttachmentSubscription()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;

        this.SenderName = "SampleSystem";
        this.SenderEmail = "Attachment@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = [new SecurityItemSourceLambda()];
        this.RecipientsSelectorMode = RecipientsSelectorMode.Union;
        this.IncludeAttachments = true;
        this.AttachmentLambda = new AttachmentLambda();
    }
}
