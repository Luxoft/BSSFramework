using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerTemplateImpl;

/// <summary>
/// Example for showing customizing of implementation of IRazorTemplate
/// </summary>
public class RazorTemplateImplSubscription : Subscription<Domain.Employee.Employee, RazorTemplateImpl>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override MailAddress Sender { get; } = new("RazorTemplateImplSubscription@luxoft.com", "SampleSystem");

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
