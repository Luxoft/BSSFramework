using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

/// <summary>
/// Example for showing customizing of implementation of IRazorTemplate
/// </summary>
public class RazorTemplateImplSubscription : Subscription<Domain.Employee, RazorTemplateImpl>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "RazorTemplateImplSubscription@luxoft.com";

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
