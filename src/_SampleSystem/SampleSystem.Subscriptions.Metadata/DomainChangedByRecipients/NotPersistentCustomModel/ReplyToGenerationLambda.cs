using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class ReplyToGenerationLambda : GenerationLambdaBase<Domain.Country>
{
    public ReplyToGenerationLambda()
    {
        this.DomainObjectChangeType = Framework.Subscriptions.DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Country> versions) =>
    [
        new NotificationMessageGenerationInfo(
            "replayTo@luxoft.com",
            new CustomNotificationModel(context, versions.Current),
            new CustomNotificationModel(context, versions.Previous))
    ];
}
