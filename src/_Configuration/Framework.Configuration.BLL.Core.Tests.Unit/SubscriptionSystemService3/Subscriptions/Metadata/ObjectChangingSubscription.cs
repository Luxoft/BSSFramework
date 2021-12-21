using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    internal sealed class ObjectChangingSubscription : SubscriptionMetadata<object, object, RazorTemplate<object>>
    {
        public ObjectChangingSubscription()
        {
            this.SenderName = "SampleSystem";
            this.SenderEmail = "SampleSystem@luxoft.com";
            this.ConditionLambda = new ConditionLambda();
            this.GenerationLambda = new GenerationLambda();
            this.CopyGenerationLambda = new CopyGenerationLambda();
            this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
            this.RecepientsSelectorMode = RecepientsSelectorMode.RolesExceptGeneration;
            this.SubBusinessRoleIds = new[] { BusinessRole.Administrator };
            this.SendIndividualLetters = true;
            this.ExcludeCurrentUser = true;
            this.IncludeAttachments = true;
            this.AllowEmptyListOfRecipients = true;
        }

        internal void SetSenderName(string senderName)
        {
            this.SenderName = senderName;
        }
    }
}
