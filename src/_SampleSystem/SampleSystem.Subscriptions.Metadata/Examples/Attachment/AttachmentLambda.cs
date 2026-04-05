using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public sealed class AttachmentLambda : AttachmentLambdaBase<Domain.Employee>
{
    public const string AttachmentName = "test.txt";

    /// <summary>
    /// Initializes a new instance of the <see cref="AttachmentLambda"/> class.
    /// </summary>
    public AttachmentLambda()
    {
        this.Lambda = GetAttachments;
    }

    private static System.Net.Mail.Attachment[] GetAttachments(
            IServiceProvider service,
            DomainObjectVersions<Domain.Employee> versions) =>
    [
        // Attachments could be get from any storage: Assembly Resources, Database, File system
        new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName)
        {
            // If ContentId not set .NET generate new GUID https://github.com/Microsoft/referencesource/blob/master/System/net/System/Net/mail/Attachment.cs
            ContentId = "testId@luxoft.com"
        }
    ];
}
