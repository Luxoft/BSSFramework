using CommonFramework;

namespace Framework.Notification.Domain;

public record Notification(NotificationTechnicalInformation TechnicalInformation, System.Net.Mail.MailMessage Message)
{
    public Notification(
        string subject,
        string body,
        IEnumerable<string> recipients,
        IEnumerable<Attachment> attachments,
        string sender,
        string messageTemplateCode)
        : this(
            new NotificationTechnicalInformation(messageTemplateCode, string.Empty, null),
            new System.Net.Mail.MailMessage { From = new System.Net.Mail.MailAddress(sender), Body = body, IsBodyHtml = body.StartsWith("<html") })
    {
        this.Message.Attachments.AddRange(attachments.Select(x => new System.Net.Mail.Attachment(new MemoryStream(x.Data.ToArray()), x.Filename)));
        this.Message.Subject = subject;
        this.Message.To.AddRange(recipients.Select(x => new System.Net.Mail.MailAddress(x)));
    }
}
