using System.Text;

using Framework.Configuration.Core;

using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.Examples.Attachment;

public sealed class AttachmentLambda : AttachmentLambdaBase<Domain.Employee>
{
    public const string AttachmentName = "test.txt";

    /// <summary>
    /// Initializes a new instance of the <see cref="AttachmentLambda"/> class.
    /// </summary>
    public AttachmentLambda()
    {
        this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Update;
        this.Lambda = GetAttachments;
    }

    private static System.Net.Mail.Attachment[] GetAttachments(
            ISampleSystemBLLContext context,
            DomainObjectVersions<Domain.Employee> versions)
    {
        return new[]
               {
                       // Attachments could be get from any storage: Assembly Resources, Database, File system
                       new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName)
                       {
                               // If ContentId not set .NET generate new GUID https://github.com/Microsoft/referencesource/blob/master/System/net/System/Net/mail/Attachment.cs
                               ContentId = "testId@luxoft.com"
                       }
               };
    }
}
