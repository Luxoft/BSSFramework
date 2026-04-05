using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance;

/// <summary>
/// Example for showing customizing of inheritance of Razor
/// </summary>
public class RazorInheritanceSubscriptionMetadata : SubscriptionMetadata<Domain.Employee, RazorInheritanceSubscription, _Examples_RazorInheritance_MessageTemplate_cshtml>
{
    public override string SenderName { get; } = "SampleSystem";

    public override string SenderEmail { get; } = "RazorInheritanceSubscription@luxoft.com";
}
