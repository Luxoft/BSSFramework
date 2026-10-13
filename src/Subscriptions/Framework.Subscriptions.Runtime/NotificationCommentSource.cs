using System.Net.Mail;

namespace Framework.Subscriptions;

public class NotificationCommentSource : INotificationCommentSource
{
    public string GetComment(MailMessage mailMessage) => string.Join(";", mailMessage.Attachments.Select(x => x.Name));
}
