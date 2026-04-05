using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public sealed class CopyGenerationLambda : GenerationLambdaBase<Domain.Country>
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
            DomainObjectVersions<Domain.Country> versions) => [new("tester@luxoft.com", versions.Current, versions.Previous)];
}
