namespace Framework.Notification;

public interface ISentNotificationLogger
{
    Task LogAsync(Domain.Notification notification, CancellationToken ct);
}
