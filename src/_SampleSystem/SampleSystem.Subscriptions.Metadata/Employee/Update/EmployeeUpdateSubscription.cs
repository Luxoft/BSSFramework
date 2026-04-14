using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public class EmployeeUpdateSubscription : Subscription<Domain.Employee.Employee, _Employee_Update_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override MailAddress Sender { get; } = new("SampleSystem@luxoft.com", "SampleSystem");

    public override bool InlineAttachments { get; } = false;

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetReplyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("replayTo@luxoft.com", versions);
    }
}
