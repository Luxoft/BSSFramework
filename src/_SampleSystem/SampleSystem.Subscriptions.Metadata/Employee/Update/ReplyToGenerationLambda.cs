using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class ReplyToGenerationLambda : GenerationLambdaBase<Domain.Employee>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationLambda"/> class.
    /// </summary>
    public ReplyToGenerationLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
        IServiceProvider _,
        DomainObjectVersions<Domain.Employee> versions) => [new NotificationMessageGenerationInfo("replayTo@luxoft.com", versions.Previous, versions.Current)];
}
