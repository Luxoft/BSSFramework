namespace Framework.Persistent;

/// <summary>
/// Константы, описывающие правила для вычисления принципалов по роле
/// </summary>
public enum NotificationExpandType
{
    /// <summary>
    /// Находятся только принципалы напрямую связанные с ролью или если для роли не указан контекст
    /// </summary>
    DirectOrEmpty = 0,

    /// <summary>
    /// Находятся только принципалы напрямую связанные с ролью
    /// </summary>
    Direct = 1,

    /// <summary>
    /// Находится первый родитель Entity (или сам Entity), который содержит хоть один пермишен с определенной ролью
    /// И по данному родителю находятся все принципалы по правилу Direct
    /// </summary>
    DirectOrFirstParent = 2,

    /// <summary>
    /// Включает все предыдущие виды (аналог авторизации)
    /// </summary>
    DirectOrFirstParentOrEmpty = 3,

    /// <summary>
    /// Находяться все принципалы, которые имеют доступ
    /// </summary>
    All = 4
}


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
