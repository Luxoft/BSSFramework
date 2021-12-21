namespace Framework.Notification
{
    public interface INotificationServiceContainer<out TNotificationService>
        //where TNotificationService : INotificationService
    {
        TNotificationService NotificationService { get; }
    }
}