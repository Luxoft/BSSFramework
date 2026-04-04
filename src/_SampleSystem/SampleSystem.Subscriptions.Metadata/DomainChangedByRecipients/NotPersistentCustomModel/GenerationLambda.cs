using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public sealed class GenerationLambda : GenerationLambdaBase<Domain.Country>
{
    public GenerationLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<Domain.Country> versions) =>
    [
        new(
            "tester@luxoft.com",
            new CustomNotificationModel(service, versions.Current),
            new CustomNotificationModel(service, versions.Previous))
    ];
}
