using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public sealed class GenerationLambda : GenerationLambdaBase<Domain.Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationLambda"/> class.
    /// </summary>
    public GenerationLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Create;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<Domain.Country> versions) => [new("tester@luxoft.com", versions.Current, versions.Previous)];
}
