using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator;

public sealed class AttachmentTemplateEvaluatorSubscription
        : SubscriptionMetadata<Domain.Employee, _Examples_AttachmentTemplateEvaluator_MessageTemplate_cshtml>
{
    /// <summary>
    /// Sample with template attachment
    /// </summary>
    public AttachmentTemplateEvaluatorSubscription()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;

        this.SenderName = "SampleSystem";
        this.SenderEmail = "AttachmentTemplateEvaluator@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = [new SecurityItemSourceLambda()];
        this.RecipientsSelectorMode = RecipientsSelectorMode.Union;
        this.IncludeAttachments = true;
        this.AttachmentLambda = new AttachmentLambdaTemplateEvaluator();
    }
}
