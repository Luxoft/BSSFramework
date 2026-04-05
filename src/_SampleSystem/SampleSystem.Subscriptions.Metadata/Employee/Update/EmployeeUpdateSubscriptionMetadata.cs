using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public class EmployeeUpdateSubscriptionMetadata
    : SubscriptionMetadata<Domain.Employee, EmployeeUpdateSubscription, _Employee_Update_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "SampleSystem@luxoft.com";


    public override bool SendIndividualLetters { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool IncludeAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = false;
}
