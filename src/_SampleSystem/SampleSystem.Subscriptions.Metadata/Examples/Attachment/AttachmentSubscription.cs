using ASP;

using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public sealed class AttachmentSubscription
        : SubscriptionMetadataBase<Domain.Employee, _Examples_Attachment_MessageTemplate_cshtml>
{
    /// <summary>
    /// Sample with inline attachment
    /// </summary>
    public AttachmentSubscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "Attachment@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
        this.RecepientsSelectorMode = Framework.Configuration.RecepientsSelectorMode.Union;
        this.SubBusinessRoleIds = Enumerable.Empty<Guid>();
        this.IncludeAttachments = true;
        this.AttachmentLambda = new AttachmentLambda();
    }
}
