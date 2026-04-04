using Framework.Subscriptions.Metadata;

using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

/// <summary>
/// Example for showing customizing of implementation of IRazerTemplate
/// </summary>
public sealed class RazerTemplateImplSubscription : SubscriptionMetadata<Domain.Employee, RazorTemplateImpl>
{
    public RazerTemplateImplSubscription()
    {
        this.SenderName = "SampleSystem";
        this.SenderEmail = "RazerTemplateImplSubscription@luxoft.com";
        this.ConditionLambda = new ConditionLambda();
        this.GenerationLambda = new GenerationLambda();
        this.CopyGenerationLambda = new CopyGenerationLambda();
        this.SecurityItemSourceLambdas = [new SecurityItemSourceLambda()];
    }
}
