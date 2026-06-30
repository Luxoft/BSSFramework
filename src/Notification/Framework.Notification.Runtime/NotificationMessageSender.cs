using System.Net.Mail;

using Framework.Core;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class NotificationMessageSender(
    IMessageSender<MailMessage> mailMessageSender,
    ILogger<NotificationMessageSender> logger,
    ISentNotificationLogger? sentNotificationLogger = null) : IMessageSender<Notification.Domain.Notification>
{
    public async Task SendAsync(Domain.Notification notification, CancellationToken ct)
    {
        if (notification.Message.To.Count == 0)
        {
            logger.LogWarning(
                "Recipients for notification {TemplateCode} were not found - notification was redirected to support",
                notification.TechnicalInformation.MessageTemplateCode);
        }

        await mailMessageSender.SendAsync(notification.Message, ct);

        if (sentNotificationLogger is not null)
        {
            await sentNotificationLogger.LogAsync(notification, ct);
        }
    }
}

