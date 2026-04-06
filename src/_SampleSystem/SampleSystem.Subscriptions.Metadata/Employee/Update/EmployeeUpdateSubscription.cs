using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public class EmployeeUpdateSubscription : Subscription<Domain.Employee, _Employee_Update_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "SampleSystem@luxoft.com";


    public override bool SendIndividualLetters { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool IncludeAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = false;

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetReplyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("replayTo@luxoft.com", versions);
    }
}
