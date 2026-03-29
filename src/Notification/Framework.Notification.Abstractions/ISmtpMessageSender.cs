using Framework.Notification.DTO;

namespace Framework.Notification;

public interface ISmtpMessageSender
{
    Task SendAsync(NotificationEventDTO message, CancellationToken cancellationToken = default);
}
