using SecuritySystem;

namespace Framework.Configuration.Domain;

/// <summary>
/// Связь между подпиской и бизнес-ролью
/// </summary>
public class SubBusinessRole
{
    public SubBusinessRole()
    {
    }

    public SubBusinessRole(Subscription subscription)
    {
        this.Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        ((ICollection<SubBusinessRole>)this.Subscription.SubBusinessRoles).Add(this);
    }

    public virtual Subscription Subscription { get; set; }

    /// <summary>
    /// ID бизнес-роли
    /// </summary>
    public virtual SecurityRole SecurityRole { get; set;
    }
}
