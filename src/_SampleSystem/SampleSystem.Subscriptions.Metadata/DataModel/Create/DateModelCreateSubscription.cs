using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create;

public class DateModelCreateSubscription : Subscription<DateModel, _DataModel_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override MailAddress Sender { get; } = new("DateModelCreateSampleSystem@luxoft.com", "DateModelCreateSampleSystem");

    public override bool SendIndividualLetters { get; } = true;

    public override bool InlineAttachments { get; } = false;

    public override IEnumerable<NotificationMessageGenerationInfo<DateModel>> GetTo(IServiceProvider _, DomainObjectVersions<DateModel> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
