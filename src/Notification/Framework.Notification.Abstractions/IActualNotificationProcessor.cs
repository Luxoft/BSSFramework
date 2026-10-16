namespace Framework.Notification;

public interface IActualNotificationProcessor
{
    Notification.Domain.Notification GetActualNotification(Notification.Domain.Notification baseNotification);
}
