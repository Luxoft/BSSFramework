using Anch.Core;

using Framework.Notification.Domain;
using Framework.Notification.MailMessageModifier;

namespace Framework.Notification;

public static class NotificationExtensions
{
    public static Domain.Notification ApplyModifiers(this Domain.Notification notification, IEnumerable<IMailMessageModifier> mailMessageModifiers)
    {
        var newMailMessage = notification.Message.Clone();

        mailMessageModifiers.Foreach(m => m.Modify(newMailMessage));

        return notification with { Message = newMailMessage };
    }
}
