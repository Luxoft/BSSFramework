using Framework.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class RootNotificationMessageSender(
    [FromKeyedServices(RootNotificationMessageSender.InnerKey)]
    IMessageSender<Notification.Domain.Notification> innerMessageSender,
    ILogger<RootNotificationMessageSender> logger,
    IActualNotificationProcessor actualNotificationProcessor,
    ISentNotificationLogger sentNotificationLogger) : IMessageSender<Notification.Domain.Notification>
{
    public const string InnerKey = "Inner";

    public async Task SendAsync(Domain.Notification baseNotification, CancellationToken ct)
    {
        if (baseNotification.Message.To.Count == 0)
        {
            logger.LogWarning(
                "Recipients for notification {TemplateCode} were not found - notification was redirected to support",
                baseNotification.TechnicalInformation.MessageTemplateCode);
        }

        var actualNotification = actualNotificationProcessor.GetActualNotification(baseNotification);

        await innerMessageSender.SendAsync(actualNotification, ct);

        await sentNotificationLogger.LogAsync(actualNotification, ct);
    }
}
