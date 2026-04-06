using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public class AttachmentSubscription : ISubscription<Domain.Employee>
{
    public const string AttachmentName = "test.txt";

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public IEnumerable<System.Net.Mail.Attachment> GetAttachments(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName)
                     {
                         // If ContentId not set .NET generate new GUID https://github.com/Microsoft/referencesource/blob/master/System/net/System/Net/mail/Attachment.cs
                         ContentId = "testId@luxoft.com"
                     };
    }
}
