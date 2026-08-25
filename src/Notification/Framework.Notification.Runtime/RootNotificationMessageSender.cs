using Framework.Core;
using Framework.Notification.MailMessageModifier;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class RootNotificationMessageSender(
    [FromKeyedServices("Native")] IMessageSender<Notification.Domain.Notification> nativeMessageSender,
    ILogger<RootNotificationMessageSender> logger,
    IEnumerable<IMailMessageModifier> mailMessageModifiers,
    [FromKeyedServices(IMailMessageModifier.LoggerKey)]
    IEnumerable<IMailMessageModifier> loggerMailMessageModifiers,
    ISentNotificationLogger? sentNotificationLogger = null) : IMessageSender<Notification.Domain.Notification>
{
    public async Task SendAsync(Domain.Notification baseNotification, CancellationToken ct)
    {
        if (baseNotification.Message.To.Count == 0)
        {
            logger.LogWarning(
                "Recipients for notification {TemplateCode} were not found - notification was redirected to support",
                baseNotification.TechnicalInformation.MessageTemplateCode);
        }

        var actualNotification = baseNotification.ApplyModifiers(mailMessageModifiers);

        await nativeMessageSender.SendAsync(actualNotification, ct);

        if (sentNotificationLogger is not null)
        {
            var loggerNotification = actualNotification.ApplyModifiers(loggerMailMessageModifiers);

            await sentNotificationLogger.LogAsync(loggerNotification, ct);
        }
    }
}
