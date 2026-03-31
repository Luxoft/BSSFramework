using Framework.Subscriptions;
using Framework.Subscriptions.Domain;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class GenerationLambda : GenerationLambdaBase<Domain.Country>
{
    public GenerationLambda()
    {
        this.DomainObjectChangeType = Framework.Subscriptions.DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Country> versions)
    {
        return
        [
            new NotificationMessageGenerationInfo(
                                                             "tester@luxoft.com",
                                                             new CustomNotificationModel(context, versions.Current),
                                                             new CustomNotificationModel(context, versions.Previous))
        ];
    }
}
