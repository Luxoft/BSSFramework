using System;
using System.Collections.ObjectModel;
using System.Net.Mail;

using Framework.Core;
using Framework.Notification;
using Framework.Notification.DTO;

namespace Framework.DomainDriven.ServiceModel
{
    /// <summary>
    /// Контект для уведомления/нотификаций
    /// </summary>
    public interface INotificationContext : IMailAddressContainer
    {
        /// <summary>
        /// Sender для отправки нотификаций в biztalk через MSMQ
        /// </summary>
        IMessageSender<NotificationEventDTO> MSMQNotificationMessageSender { get; }

        /// <summary>
        /// Список получателей серверных ошибок
        /// </summary>
        ReadOnlyCollection<string> ExceptionReceivers { get; }
    }

    public class NotificationContext : INotificationContext
    {
        public NotificationContext(IMessageSender<NotificationEventDTO> notificationMessageSender, string sender, params string[] receivers)
            : this(notificationMessageSender, new MailAddress(sender, sender), receivers)
        {
        }

        public NotificationContext(IMessageSender<NotificationEventDTO> msmqNotificationMessageSender, MailAddress sender, params string[] receivers)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (receivers == null) throw new ArgumentNullException(nameof(receivers));
            if (string.IsNullOrWhiteSpace(sender.DisplayName)) throw new System.ArgumentException("Not initialize sender name in NotificationContext");

            this.MSMQNotificationMessageSender = msmqNotificationMessageSender ?? throw new ArgumentNullException(nameof(msmqNotificationMessageSender));
            this.Sender = sender;
            this.ExceptionReceivers = receivers.ToReadOnlyCollection();
        }

        /// <inheritdoc />
        public IMessageSender<NotificationEventDTO> MSMQNotificationMessageSender { get; }

        public MailAddress Sender { get; }

        /// <inheritdoc />
        public ReadOnlyCollection<string> ExceptionReceivers { get; }
    }
}
