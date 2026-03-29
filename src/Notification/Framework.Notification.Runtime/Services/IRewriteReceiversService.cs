using System.Net.Mail;
using Framework.Notification.DTO;

namespace Framework.NotificationCore.Services;

public interface IRewriteReceiversService
{
    void RewriteToRecipients(MailMessage message, NotificationEventDTO dto);

    void RewriteCopyRecipients(MailMessage message, NotificationEventDTO dto);

    void RewriteReplyTo(MailMessage message, NotificationEventDTO dto);
}
