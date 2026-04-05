using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions;

/// <summary>
/// Элемент типизированного контекста
/// </summary>
public class SubscriptionSecurityItem
{
    private NotificationExpandType expandType;

    private SubscriptionLambda source;

    public virtual SubscriptionLambda Source
    {
        get => this.source;
        set => this.source = value;
    }

    /// <summary>
    /// Тип Expand Type, отображающий расширение прав по дереву
    /// </summary>
    public virtual NotificationExpandType ExpandType
    {
        get => this.expandType;
        set => this.expandType = value;
    }
}
