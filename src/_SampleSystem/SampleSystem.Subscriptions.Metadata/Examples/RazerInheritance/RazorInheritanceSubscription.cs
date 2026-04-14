using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.RazerInheritance;

/// <summary>
/// Example for showing customizing of inheritance of Razor
/// </summary>
public class RazorInheritanceSubscription : Subscription<Domain.Employee.Employee, _Examples_RazorInheritance_MessageTemplate_cshtml>
{
    public override MailAddress Sender { get; } = new ("RazorInheritanceSubscription@luxoft.com", "SampleSystem");

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
