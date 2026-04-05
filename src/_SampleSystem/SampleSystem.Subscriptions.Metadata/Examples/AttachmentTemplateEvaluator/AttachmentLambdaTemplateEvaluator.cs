using System.Text;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator;

public sealed class AttachmentLambdaTemplateEvaluator : AttachmentLambdaBase<Domain.Employee>
{
    public const string AttachmentName = "report.html";

    /// <summary>
    /// Initializes a new instance of the <see cref="AttachmentLambdaTemplateEvaluator"/> class.
    /// </summary>
    public AttachmentLambdaTemplateEvaluator()
    {
        this.Lambda = GetAttachments;
    }

    private static System.Net.Mail.Attachment[] GetAttachments(
            IServiceProvider service,
            DomainObjectVersions<Domain.Employee> versions)
    {
        var template = Encoding.UTF8.GetBytes($"Hello world! {versions.Current.NameNative}");

        return [new System.Net.Mail.Attachment(new MemoryStream(template), AttachmentName)];
    }
}
