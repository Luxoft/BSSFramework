using Framework.Configuration.Core;
using Framework.Notification;

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
        this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Create;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Country> versions)
    {
        return new[] { new NotificationMessageGenerationInfo("tester@luxoft.com", versions.Current, versions.Previous) };
    }
}
