using System;
using System.Net.Mail;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification
{
    public class NotificationMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<Framework.Notification.Notification>
    {
        private readonly MailAddress defaultSender;

        private readonly IMessageSender<NotificationEventDTO> notificationEventSender;


        public NotificationMessageSender(IConfigurationBLLContext context, IMessageSender<NotificationEventDTO> notificationEventSender, MailAddress defaultSender)
                : base(context)
        {
            this.defaultSender = defaultSender ?? throw new ArgumentNullException(nameof(defaultSender));
            this.notificationEventSender = notificationEventSender ?? throw new ArgumentNullException(nameof(notificationEventSender));
        }

        public void Send(Framework.Notification.Notification notification)
        {
            notification.Message.Sender = notification.Message.Sender ?? this.defaultSender;
            this.notificationEventSender.Send(new NotificationEventDTO(notification));
        }
    }
}
