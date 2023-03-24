using ASP;
using System;
using System.Linq;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class Subscription
        : SubscriptionWithCustomModelMetadataBase<Domain.Country, CustomNotificationModel, _DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml>
{
    public Subscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "SampleSystem@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.ReplyToGenerationLambda = new ReplyToGenerationLambda();
        this.AttachmentLambda = new AttachmentLambda();
        this.RecepientsSelectorMode = Framework.Configuration.RecepientsSelectorMode.Union;
        this.SubBusinessRoleIds = Enumerable.Empty<Guid>();
        this.SendIndividualLetters = true;
        this.ExcludeCurrentUser = true;
        this.IncludeAttachments = true;
        this.AllowEmptyListOfRecipients = false;
    }
}
