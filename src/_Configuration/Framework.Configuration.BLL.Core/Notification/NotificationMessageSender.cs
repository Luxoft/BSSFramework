using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification
{
    public class NotificationMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<Framework.Notification.Notification>
    {
        private readonly IDefaultMailSenderContainer defaultMailSenderContainer;

        private readonly IMessageSender<NotificationEventDTO> notificationEventSender;


        public NotificationMessageSender(IConfigurationBLLContext context, IMessageSender<NotificationEventDTO> notificationEventSender, IDefaultMailSenderContainer defaultMailSenderContainer)
                : base(context)
        {
            this.defaultMailSenderContainer = defaultMailSenderContainer ?? throw new ArgumentNullException(nameof(defaultMailSenderContainer));
            this.notificationEventSender = notificationEventSender ?? throw new ArgumentNullException(nameof(notificationEventSender));
        }

        public void Send(Framework.Notification.Notification notification)
        {
            notification.Message.Sender = notification.Message.Sender ?? this.defaultMailSenderContainer.DefaultSender;

            this.notificationEventSender.Send(new NotificationEventDTO(notification));
        }
    }
}
