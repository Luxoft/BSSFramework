using System;
using Framework.Core;

namespace Framework.Notification
{
    public interface INotificationService
    {
        IMessageSender<MessageTemplateNotification> MainSender { get; }

        IMessageSender<Notification> NotificationSender { get; }

        IMessageSender<MessageTemplateNotification> SubscriptionSender { get; }
    }
}
