namespace Framework.Authorization.Notification;

public static class NotificationExpandTypeExtensions
{
    public static bool IsHierarchical(this NotificationExpandType notificationExpandType)
    {
        switch (notificationExpandType)
        {
            case NotificationExpandType.DirectOrFirstParent:
            case NotificationExpandType.DirectOrFirstParentOrEmpty:
            case NotificationExpandType.All:
                return true;

            default:
                return false;
        }
    }

    public static NotificationExpandType WithoutHierarchical(this NotificationExpandType notificationExpandType)
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

    public static bool AllowEmpty(this NotificationExpandType notificationExpandType)
    {
        switch (notificationExpandType)
        {
            case NotificationExpandType.DirectOrEmpty:
            case NotificationExpandType.DirectOrFirstParentOrEmpty:
            case NotificationExpandType.All:
                return true;

            default:
                return false;
        }
    }
}
