using System;
using System.Linq;

using ASP;

namespace SampleSystem.Subscriptions.Metadata.Country.Create
{
    /// <inheritdoc />
    public sealed class CountryCreateSubscription
        : SubscriptionMetadataBase<Domain.Country, _Country_Create_MessageTemplate_cshtml>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryUpdateSubscription"/> class.
        /// </summary>
        public CountryCreateSubscription()
        {
            this.SenderName = "SampleSystem";
            this.SenderEmail = "SampleSystem@luxoft.com";
            this.ConditionLambda = new ConditionLambda();
            this.GenerationLambda = new GenerationLambda();
            this.CopyGenerationLambda = new CopyGenerationLambda();
            this.RecepientsSelectorMode = Framework.Configuration.RecepientsSelectorMode.Union;
            this.SubBusinessRoleIds = Enumerable.Empty<Guid>();
            this.SendIndividualLetters = true;
            this.ExcludeCurrentUser = true;
            this.IncludeAttachments = false;
            this.AllowEmptyListOfRecipients = false;
        }
    }
}
