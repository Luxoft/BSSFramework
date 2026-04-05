using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Employee.Update;

/// <inheritdoc />
public sealed class CopyGenerationLambda : GenerationLambdaBase<Domain.Employee>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CopyGenerationLambda"/> class.
    /// </summary>
    public CopyGenerationLambda()
    {
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<Domain.Employee> versions) => [new("tester@luxoft.com", versions.Previous, versions.Current)];
}
