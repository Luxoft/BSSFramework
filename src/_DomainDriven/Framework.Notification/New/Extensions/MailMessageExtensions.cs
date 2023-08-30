using System.Net.Mail;
using Framework.Core;

namespace Framework.Notification.New;

public static class MailMessageExtensions
{
    public static MailMessage ToMailMessage(this Message message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        var mailMessage = new MailMessage { Subject = message.Subject, Body = message.Body, From = message.Sender, IsBodyHtml = message.IsBodyHtml };

        mailMessage.To.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.To));
        mailMessage.CC.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.Copy));

        mailMessage.ReplyToList.AddRange(message.Receivers.Where(r => r.Role == ReceiverRole.ReplyTo));

        mailMessage.Attachments.AddRange(message.Attachments.Select(a => ToMailAttachment(a)));

        return mailMessage;
    }

    private static System.Net.Mail.Attachment ToMailAttachment(Attachment attachment)
    {
        var mailAttachment = new System.Net.Mail.Attachment(new MemoryStream(attachment.Data), attachment.Filename)
                             {
                                     ContentId = attachment.ContentId
                             };
        mailAttachment.ContentDisposition.Inline = attachment.IsInline;

        return mailAttachment;
    }
}
