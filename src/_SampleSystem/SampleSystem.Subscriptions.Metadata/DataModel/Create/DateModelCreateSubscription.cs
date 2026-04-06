using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create;

public class DateModelCreateSubscription : Subscription<DateModel, _DataModel_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override string SenderName { get; } = "DateModelCreateSampleSystem";

    public override string SenderEmail { get; } = "DateModelCreateSampleSystem@luxoft.com";

    public override bool SendIndividualLetters { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool IncludeAttachments { get; } = false;

    public override IEnumerable<NotificationMessageGenerationInfo<DateModel>> GetTo(IServiceProvider _, DomainObjectVersions<DateModel> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
