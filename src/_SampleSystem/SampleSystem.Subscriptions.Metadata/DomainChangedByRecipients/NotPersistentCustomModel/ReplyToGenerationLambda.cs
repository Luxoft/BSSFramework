using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class ReplyToGenerationLambda : GenerationLambdaBase<Domain.Country>
{
    public ReplyToGenerationLambda()
    {
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<Domain.Country> versions) =>
    [
        new(
            "replayTo@luxoft.com",
            new CustomNotificationModel(service, versions.Current!),
            new CustomNotificationModel(service, versions.Previous!))
    ];
}
