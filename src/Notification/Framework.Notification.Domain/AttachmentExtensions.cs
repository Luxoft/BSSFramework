namespace Framework.Notification.Domain;

public static class AttachmentExtensions
{
    extension(System.Net.Mail.Attachment attachment)
    {
        public bool IsInline
        {
            get => attachment.ContentDisposition!.Inline;
            set => attachment.ContentDisposition!.Inline = value;
        }
    }
}
