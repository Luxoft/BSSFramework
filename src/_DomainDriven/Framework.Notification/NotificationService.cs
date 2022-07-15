using System;
using Framework.Core;

namespace Framework.Notification
{
    /// <summary>
    /// Notification service
    /// </summary>
    public class NotificationService : INotificationService
    {
        public NotificationService(IMessageSender<MessageTemplateNotification> mainSender, IMessageSender<Notification> notificationSender, IMessageSender<MessageTemplateNotification> subscriptionSender)
        {
            if (mainSender == null) throw new ArgumentNullException(nameof(mainSender));
            if (subscriptionSender == null) throw new ArgumentNullException(nameof(subscriptionSender));

            this.MainSender = mainSender;
            this.NotificationSender = notificationSender;
            this.SubscriptionSender = subscriptionSender;
        }

        /// <summary>
        /// Template message sender
        /// </summary>
        public IMessageSender<MessageTemplateNotification> MainSender { get; private set; }

        /// <summary>
        /// Subscription message sender
        /// </summary>
        public IMessageSender<MessageTemplateNotification> SubscriptionSender { get; private set; }

        /// <summary>
        /// Notification message sender
        /// </summary>
        public IMessageSender<Notification> NotificationSender { get; private set; }
    }
}
