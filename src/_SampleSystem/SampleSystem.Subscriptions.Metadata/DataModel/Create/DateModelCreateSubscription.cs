using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create;

public sealed class DateModelCreateSubscription : SubscriptionMetadata<DateModel, _DataModel_Create_MessageTemplate_cshtml>
{
    public DateModelCreateSubscription()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Create;

        this.SenderName = "DateModelCreateSampleSystem";
        this.SenderEmail = "DateModelCreateSampleSystem@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.RecipientsSelectorMode = RecipientsSelectorMode.Union;
        this.SendIndividualLetters = true;
        this.ExcludeCurrentUser = true;
        this.IncludeAttachments = false;
        this.AllowEmptyListOfRecipients = false;
    }
}
