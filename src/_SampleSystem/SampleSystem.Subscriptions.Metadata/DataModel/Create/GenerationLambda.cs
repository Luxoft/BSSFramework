using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create;

public sealed class GenerationLambda : GenerationLambdaBase<DateModel>
{
    public GenerationLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Create;
        this.Lambda = this.GetRecipients;
    }

    private NotificationMessageGenerationInfo[] GetRecipients(
            IServiceProvider service,
            DomainObjectVersions<DateModel> versions) => [new("tester@luxoft.com", versions.Previous, versions.Current)];
}
