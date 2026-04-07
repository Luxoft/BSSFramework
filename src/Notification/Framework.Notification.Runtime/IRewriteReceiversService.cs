using System.Net.Mail;

namespace Framework.Notification;

public interface IRewriteReceiversService
{
    void RewriteRecipients(MailMessage message);
}
