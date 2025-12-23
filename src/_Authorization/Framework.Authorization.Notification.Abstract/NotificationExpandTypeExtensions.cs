namespace Framework.Authorization.Notification;

public static class NotificationExpandTypeExtensions
{
    extension(NotificationExpandType notificationExpandType)
    {
        public bool IsHierarchical()
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

        public bool AllowEmpty()
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
}
