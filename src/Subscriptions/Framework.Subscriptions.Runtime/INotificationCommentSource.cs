using System.Net.Mail;

namespace Framework.Subscriptions;

public interface INotificationCommentSource
{
    string GetComment(MailMessage mailMessage);
}
