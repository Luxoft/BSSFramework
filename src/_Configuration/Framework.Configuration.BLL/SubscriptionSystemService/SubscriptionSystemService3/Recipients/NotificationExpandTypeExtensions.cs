using SecuritySystem.Notification.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Recipients;

public static class NotificationExpandTypeExtensions
{
    extension(NotificationExpandType notificationExpandType)
    {
        public NotificationExpandType WithoutHierarchical()
        {
            switch (notificationExpandType)
            {
                case NotificationExpandType.DirectOrFirstParent:
                    return NotificationExpandType.Direct;

                case NotificationExpandType.DirectOrFirstParentOrEmpty:
                case NotificationExpandType.All:
                    return NotificationExpandType.DirectOrEmpty;

                default:
                    return notificationExpandType;
            }
        }
    }
}
