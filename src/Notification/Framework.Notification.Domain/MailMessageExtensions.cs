using System.Net.Mail;

using CommonFramework;

namespace Framework.Notification.Domain;

public static class MailMessageExtensions
{
    public static MailMessage ToMailMessage(this Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        var mailMessage = new MailMessage { Subject = message.Subject, Body = message.Body, From = message.Sender, IsBodyHtml = message.IsBodyHtml };

        mailMessage.To.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.To).Select(info => info.MailAddress));
        mailMessage.CC.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.Copy).Select(info => info.MailAddress));
        mailMessage.ReplyToList.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.ReplyTo).Select(info => info.MailAddress));

        mailMessage.Attachments.AddRange(message.Attachments.Select(ToMailAttachment));

        return mailMessage;
    }

    private static System.Net.Mail.Attachment ToMailAttachment(Attachment attachment) =>
        new(new MemoryStream(attachment.Data), attachment.Filename) { ContentId = attachment.ContentId, ContentDisposition = { Inline = attachment.IsInline } };
}
