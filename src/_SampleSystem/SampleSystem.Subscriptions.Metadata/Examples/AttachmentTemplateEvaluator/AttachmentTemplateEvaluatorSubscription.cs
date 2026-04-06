using System.Text;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator;

public class AttachmentTemplateEvaluatorSubscription : Subscription<Domain.Employee, _Examples_AttachmentTemplateEvaluator_MessageTemplate_cshtml>
{
    public const string AttachmentName = "report.html";

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Update;

    public override string SenderName { get; } = "SampleSystem";

    public override string SenderEmail { get; } = "AttachmentTemplateEvaluator@luxoft.com";

    public override bool IncludeAttachments { get; } = true;

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<NotificationMessageGenerationInfo<Domain.Employee>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Employee> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override IEnumerable<System.Net.Mail.Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<Domain.Employee> versions)
    {
        var template = Encoding.UTF8.GetBytes($"Hello world! {versions.Current!.NameNative}");

        yield return new System.Net.Mail.Attachment(new MemoryStream(template), AttachmentName);
    }
}
