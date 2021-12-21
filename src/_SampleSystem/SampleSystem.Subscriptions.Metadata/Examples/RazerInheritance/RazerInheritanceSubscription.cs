using ASP;

using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance
{
    /// <summary>
    /// Example for showing customizing of inheritance of Razer
    /// </summary>
    public sealed class RazerInheritanceSubscription
            : SubscriptionMetadataBase<Domain.Employee, _Examples_RazerInheritance_MessageTemplate_cshtml>
    {
        public RazerInheritanceSubscription()
        {
            this.SenderName = "SampleSystem";
            this.SenderEmail = "RazerInheritanceSubscription@luxoft.com";
            this.ConditionLambda = new ConditionLambda();
            this.GenerationLambda = new GenerationLambda();
            this.CopyGenerationLambda = new CopyGenerationLambda();
            this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
        }
    }
}
