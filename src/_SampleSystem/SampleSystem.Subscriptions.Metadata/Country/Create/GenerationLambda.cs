using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.Country.Create;

/// <inheritdoc />
public sealed class GenerationLambda : GenerationLambdaBase<Domain.Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenerationLambda"/> class.
    /// </summary>
    public GenerationLambda()
    {
        this.DomainObjectChangeType = Framework.Subscriptions.DomainObjectChangeType.Create;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Country> versions)
    {
        return [new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Current, versions.Previous)];
    }
}
