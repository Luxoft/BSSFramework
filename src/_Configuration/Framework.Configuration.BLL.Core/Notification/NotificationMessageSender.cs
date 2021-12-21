using System;
using System.Net.Mail;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification
{
    public static partial class MessageSenderExtensions
    {
        /// <summary>
        /// Creates and returns notification message sender
        /// </summary>
        /// <param name="notificationEventSender">Base notification event sender</param>
        /// <param name="context">Configuration context</param>
        /// <param name="defaultSender">Default message sender</param>
        /// <returns>Notification message sender</returns>
        public static IMessageSender<Framework.Notification.Notification> ToNotificationSender(this IMessageSender<NotificationEventDTO> notificationEventSender, IConfigurationBLLContext context, MailAddress defaultSender)
        {
            if (notificationEventSender == null) throw new ArgumentNullException(nameof(notificationEventSender));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (defaultSender == null) throw new ArgumentNullException(nameof(defaultSender));

            return new NotificationMessageSender(context, notificationEventSender, defaultSender);
        }

        private class NotificationMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<Framework.Notification.Notification>
        {
            private readonly MailAddress _defaultSender;
            private readonly IMessageSender<NotificationEventDTO> _notificationEventSender;


            public NotificationMessageSender(IConfigurationBLLContext context, IMessageSender<NotificationEventDTO> notificationEventSender, MailAddress defaultSender)
                : base(context)
            {
                if (defaultSender == null) throw new ArgumentNullException(nameof(defaultSender));
                if (notificationEventSender == null) throw new ArgumentNullException(nameof(notificationEventSender));

                this._defaultSender = defaultSender;
                this._notificationEventSender = notificationEventSender;
            }

            public void Send(Framework.Notification.Notification notification, TransactionMessageMode transactionMessageMode)
            {
                notification.Message.Sender = notification.Message.Sender ?? this._defaultSender;
                this._notificationEventSender.Send(new NotificationEventDTO(notification), transactionMessageMode);
            }
        }
    }
}