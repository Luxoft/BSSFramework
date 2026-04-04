using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class Subscription : SubscriptionMetadata<Domain.Country,  _DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml>
{
    public Subscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "SampleSystem@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.ReplyToGenerationLambda = new ReplyToGenerationLambda();
        this.AttachmentLambda = new AttachmentLambda().ChangeInput<Domain.Country>();
        this.RecipientsSelectorMode = RecipientsSelectorMode.Union;
        this.SendIndividualLetters = true;
        this.ExcludeCurrentUser = true;
        this.IncludeAttachments = true;
        this.AllowEmptyListOfRecipients = false;
    }
}
