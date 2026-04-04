using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class GenerationLambda : GenerationLambdaBase<Domain.Employee>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationLambda"/> class.
    /// </summary>
    public GenerationLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<Domain.Employee> versions) => [new("tester@luxoft.com", versions.Previous, versions.Current)];
}
