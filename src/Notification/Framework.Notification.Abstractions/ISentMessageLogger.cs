using System.Net.Mail;

namespace Framework.Notification;

public interface ISentMessageLogger
{
    Task LogAsync(MailMessage mailMessage, CancellationToken cancellationToken = default);
}
