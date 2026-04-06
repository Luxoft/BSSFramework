using System.Net.Mail;
using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class Subscription : Subscription<Domain.Country, CustomNotificationModel, ASP._DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml>
{

    public const string AttachmentName = "test.txt";

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderEmail { get; } = "SampleSystem";

    public override string SenderName { get; } = "SampleSystem@luxoft.com";

    public override bool AllowEmptyListOfRecipients { get; } = false;

    public override bool IncludeAttachments { get; } = true;

    public override bool ExcludeCurrentUser { get; } = true;

    public override bool SendIndividualLetters { get; } = true;

    public override CustomNotificationModel ConvertToRenderingObject(
        IServiceProvider serviceProvider,
        Domain.Country domainObject) => new(serviceProvider, domainObject);

    public override IEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions.ChangeDomainObject(c => this.ConvertToRenderingObject(serviceProvider, c)));
    }

    public override IEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetReplyTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("replayTo@luxoft.com", versions.ChangeDomainObject(c => this.ConvertToRenderingObject(serviceProvider, c)));
    }

    public override IEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<CustomNotificationModel> versions)
    {
        yield return new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName);
    }
}
