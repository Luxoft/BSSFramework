namespace Framework.Notification;

public interface ISentNotificationLogger
{
    const string InnerKey = "Inner";

    Task LogAsync(Domain.Notification notification, CancellationToken ct);
}
