using Framework.Authorization.Notification;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Элемент типизированного контекста
/// </summary>
public class SubscriptionSecurityItem : IIdentityObject<Guid>
{
    private NotificationExpandType expandType;

    private SubscriptionLambda source;

    public SubscriptionSecurityItem()
    {
    }

    public SubscriptionSecurityItem(Subscription subscription)
    {
        this.Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        ((ICollection<SubscriptionSecurityItem>)this.Subscription.SecurityItems).Add(this);
    }

    public virtual Subscription Subscription { get; set; }

    /// <summary>
    /// Лямбла, получающая типизированный контекст для ролей подписки
    /// </summary>
    /// <remarks>
    /// Лямбда подписки типа "AuthSource"
    /// </remarks>
    [Required]
    public virtual SubscriptionLambda Source
    {
        get { return this.source; }
        set { this.source = value; }
    }

    /// <summary>
    /// Тип Expand Type, отображающий расширение прав по дереву
    /// </summary>
    public virtual NotificationExpandType ExpandType
    {
        get { return this.expandType; }
        set { this.expandType = value; }
    }

    Guid IIdentityObject<Guid>.Id => Guid.Empty;
}
