using Framework.Core;
using Framework.Notification.DTO;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class SmtpNotificationMessageSender(
    ISmtpMessageSender smtpMessageSender,
    ILogger<SmtpNotificationMessageSender> logger,
    ISentMessageLogger? sentMessageLogger = null) : IMessageSender<NotificationEventDTO>
{
    public async Task SendAsync(NotificationEventDTO message, CancellationToken cancellationToken)
    {
        try
        {
            await smtpMessageSender.SendAsync(message, cancellationToken);

            if (sentMessageLogger != null)
            {
                await sentMessageLogger.LogAsync(message, cancellationToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }
}
