using Anch.Core;

using Framework.Notification.Domain;
using Framework.Notification.MailMessageModifier;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification;

public class RootSentNotificationLogger(
    [FromKeyedServices(IMailMessageModifier.LoggerKey)]
    IEnumerable<IMailMessageModifier> mailMessageModifiers,
    [FromKeyedServices(ISentNotificationLogger.InnerKey)]
    ISentNotificationLogger? innerSentNotificationLogger = null)
    : ISentNotificationLogger
{
    public async Task LogAsync(Framework.Notification.Domain.Notification notification, CancellationToken ct)
    {
        if (innerSentNotificationLogger is not null)
        {
            var loggedMailMessage = notification.Message.Clone();

            mailMessageModifiers.Foreach(m => m.Modify(loggedMailMessage));

            await innerSentNotificationLogger.LogAsync(notification with { Message = loggedMailMessage }, ct);
        }
    }
}
