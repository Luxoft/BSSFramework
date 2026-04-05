using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

/// <summary>
/// Example for showing customizing of implementation of IRazorTemplate
/// </summary>
public class RazorTemplateImplSubscriptionMetadata : SubscriptionMetadata<Domain.Employee, RazorTemplateImplSubscription, RazorTemplateImpl>
{
    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "RazorTemplateImplSubscription@luxoft.com";
}
