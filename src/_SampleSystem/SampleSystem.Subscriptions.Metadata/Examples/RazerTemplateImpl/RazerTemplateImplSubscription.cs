using ASP;

using SampleSystem.Subscriptions.Metadata.Employee.Update;
using SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance;

/// <summary>
/// Example for showing customizing of implementation of IRazerTemplate
/// </summary>
public sealed class RazerTemplateImplSubscription
        : SubscriptionMetadataBase<Domain.Employee, RazorTemplateImpl>
{
    public RazerTemplateImplSubscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "RazerTemplateImplSubscription@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = new[] { new SecurityItemSourceLambda() };
    }
}
