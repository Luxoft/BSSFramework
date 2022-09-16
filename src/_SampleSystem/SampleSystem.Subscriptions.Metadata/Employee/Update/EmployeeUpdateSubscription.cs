using System;
using System.Linq;

using ASP;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update
{
    /// <inheritdoc />
    public sealed class EmployeeUpdateSubscription
        : SubscriptionMetadataBase<Domain.Employee, _Employee_Update_MessageTemplate_cshtml>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeUpdateSubscription"/> class.
        /// </summary>
        public EmployeeUpdateSubscription()
        {
            this.SenderName = "SampleSystem";
            this.SenderEmail = "SampleSystem@luxoft.com";
            this.ConditionLambda = new ConditionLambda();
            this.GenerationLambda = new GenerationLambda();
            this.CopyGenerationLambda = new CopyGenerationLambda();
            this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
            this.RecepientsSelectorMode = Framework.Configuration.RecepientsSelectorMode.Union;
            this.SubBusinessRoleIds = Enumerable.Empty<Guid>();
            this.SendIndividualLetters = true;
            this.ExcludeCurrentUser = true;
            this.IncludeAttachments = false;
            this.AllowEmptyListOfRecipients = false;
            this.ReplyToGenerationLambda = new ReplyToGenerationLambda();
        }
    }
}
