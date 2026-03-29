using Framework.Notification.DTO;

namespace Framework.Notification;

public interface ISentMessageLogger
{
    Task LogAsync(NotificationEventDTO notificationEventDTO, CancellationToken cancellationToken = default);
}
