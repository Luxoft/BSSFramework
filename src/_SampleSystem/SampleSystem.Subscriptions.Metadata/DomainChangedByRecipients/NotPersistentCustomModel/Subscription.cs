using System.Net.Mail;
using System.Text;

using CommonFramework.GenericRepository;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

public class Subscription(IQueryableSource queryableSource) : ISubscription<Domain.Country, CustomNotificationModel>
{
    public const string AttachmentName = "test.txt";

    public CustomNotificationModel ConvertToRenderingObject(Domain.Country domainObject) => new(queryableSource, domainObject);

    public IEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("tester@luxoft.com", versions.ChangeDomainObject(this.ConvertToRenderingObject));
    }

    public IEnumerable<NotificationMessageGenerationInfo<CustomNotificationModel>> GetReplyTo(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new("replayTo@luxoft.com", versions.ChangeDomainObject(this.ConvertToRenderingObject));
    }

    public IEnumerable<Attachment> GetAttachments(DomainObjectVersions<Domain.Country> versions)
    {
        yield return new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName);
    }
}
