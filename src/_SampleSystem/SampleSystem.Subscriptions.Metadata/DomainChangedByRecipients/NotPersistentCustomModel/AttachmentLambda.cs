using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

// аттачи генерятся по уже подменённой модели, после генерации получателей
public sealed class AttachmentLambda : AttachmentLambdaBase<CustomNotificationModel>
{
    public const string AttachmentName = "test.txt";

    public AttachmentLambda()
    {
        this.DomainObjectChangeType = DomainObjectChangeType.Update;
        this.Lambda = GetAttachments;
    }

    private static IEnumerable<System.Net.Mail.Attachment> GetAttachments(
        IServiceProvider service,
        DomainObjectVersions<CustomNotificationModel> versions)
    {
        yield return new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName);
    }
}
