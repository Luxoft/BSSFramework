using System.IO;
using System.Text;
using Framework.Configuration.Core;
using SampleSystem.BLL;

namespace SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel
{
    // аттачи генерятся по уже подменённой модели, после генерации получателей
    public sealed class AttachmentLambda : AttachmentLambdaBase<CustomNotificationModel>
    {
        public const string AttachmentName = "test.txt";

        public AttachmentLambda()
        {
            this.DomainObjectChangeType = Framework.Configuration.SubscriptionModeling.DomainObjectChangeType.Update;
            this.Lambda = GetAttachments;
        }

        private static System.Net.Mail.Attachment[] GetAttachments(
            ISampleSystemBLLContext context,
            DomainObjectVersions<CustomNotificationModel> versions)
        {
            return new[]
                   {
                       new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("Hello world!")), AttachmentName)
                   };
        }
    }
}
