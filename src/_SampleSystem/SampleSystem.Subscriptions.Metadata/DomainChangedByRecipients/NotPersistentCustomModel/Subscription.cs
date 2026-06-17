using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class Subscription : Subscription<Domain.Directories.Country, CustomNotificationModel, _DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml>
{

    public const string AttachmentName = "test.txt";

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override MailAddress Sender { get; } = new("SampleSystem@luxoft.com", "SampleSystem");

    public override async ValueTask<CustomNotificationModel> ConvertToRenderingObject(
        IServiceProvider serviceProvider,
        Domain.Directories.Country domainObject,
         CancellationToken ct) => new(serviceProvider, domainObject);

    public override IAsyncEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<Domain.Directories.Country> versions) =>

        new[] { "tester@luxoft.com" }
            .ToAsyncEnumerable()
            .Select(async (email, ct) =>
                        new NotificationMessageGenerationInfo<CustomNotificationModel>(
                            email,
                            await versions.ChangeDomainObjectAsync(c => this.ConvertToRenderingObject(serviceProvider, c, ct))));

    public override IAsyncEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetReplyTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<Domain.Directories.Country> versions) => this.InternalGetReplyTo(serviceProvider, versions);

    private async IAsyncEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> InternalGetReplyTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<Domain.Directories.Country> versions,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        yield return new("replyTo@luxoft.com", await versions.ChangeDomainObjectAsync(c => this.ConvertToRenderingObject(serviceProvider, c, ct)));
    }

    public override async IAsyncEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<CustomNotificationModel> versions)
    {
        yield return new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName);
    }
}

