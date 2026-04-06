using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentInline;

public class AttachmentInlineSubscription : Subscription<Domain.Employee, _Examples_AttachmentInline_MessageTemplate_cshtml>
{
    public const string AttachmentName = "test.txt";

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string? SenderName { get; } = "SampleSystem";

    public override string? SenderEmail { get; } = "InlineAttach@luxoft.com";

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<System.Net.Mail.Attachment> GetAttachments(IServiceProvider _, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName)
                     {
                         // If ContentId not set .NET generate new GUID https://github.com/Microsoft/referencesource/blob/master/System/net/System/Net/mail/Attachment.cs
                         ContentId = "testId@luxoft.com"
                     };
    }
}
