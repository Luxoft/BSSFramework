using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator;

public class AttachmentTemplateEvaluatorSubscription : ISubscription<Domain.Employee>
{
    public const string AttachmentName = "report.html";

    public IEnumerable<NotificationMessageGenerationInfo> GetTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions.Previous, versions.Current);
    }

    public IEnumerable<NotificationMessageGenerationInfo> GetCopyTo(DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions.Previous, versions.Current);
    }

    public IEnumerable<System.Net.Mail.Attachment> GetAttachments(DomainObjectVersions<Domain.Employee> versions)
    {
        var template = Encoding.UTF8.GetBytes($"Hello world! {versions.Current!.NameNative}");

        yield return new System.Net.Mail.Attachment(new MemoryStream(template), AttachmentName);
    }
}
