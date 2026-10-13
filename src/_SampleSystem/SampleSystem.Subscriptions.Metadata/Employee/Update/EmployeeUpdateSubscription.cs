using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public class EmployeeUpdateSubscription : Subscription<Domain.Employee.Employee, _Employee_Update_MessageTemplate_cshtml>
{
    /// <inheritdoc />
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    /// <inheritdoc />
    public override bool InlineAttachments { get; } = false;

    /// <inheritdoc />
    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Employee.Employee>> GetReplyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee.Employee> versions)
    {
        yield return new("replyTo@luxoft.com", versions);
    }
}
