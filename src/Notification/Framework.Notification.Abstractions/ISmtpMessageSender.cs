using Framework.Core;
using Framework.Notification.DTO;

namespace Framework.Notification;

public interface ISmtpMessageSender : IMessageSender<NotificationEventDTO>;
