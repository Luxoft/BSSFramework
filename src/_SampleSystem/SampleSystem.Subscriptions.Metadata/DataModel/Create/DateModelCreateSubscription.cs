using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create;

public class DateModelCreateSubscription : ISubscription<DateModel>
{
    public IEnumerable<NotificationMessageGenerationInfo<DateModel>> GetTo(DomainObjectVersions<DateModel> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
