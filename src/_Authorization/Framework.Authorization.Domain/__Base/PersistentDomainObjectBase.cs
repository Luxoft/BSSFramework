using Framework.Application.Domain;
using Framework.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

public abstract class PersistentDomainObjectBase : DomainObjectBase, IEquatable<PersistentDomainObjectBase>, IIdentityObject<Guid>
{
    private Guid id;

    protected PersistentDomainObjectBase()
    {
    }

    /// <summary>
    /// Domain object ID
    /// </summary>
    public virtual Guid Id
    {
        get => this.id;
        set => this.id = value;
    }

    /// <summary>
    /// Flag indicating that the class has not been saved to the database yet
    /// </summary>
    public virtual bool IsNew => this.Id == Guid.Empty;

    public static bool operator ==(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => ReferenceEquals(a, b) || (a is not null && b is not null && a.Equals(b));

    public static bool operator !=(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => !(a == b);

    public virtual bool Equals(PersistentDomainObjectBase? obj) => ReferenceEquals(this, obj) || (obj is not null && this.Id == obj.Id && this.Id != Guid.Empty);

    public override bool Equals(object? obj) => this.Equals(obj as PersistentDomainObjectBase);

    public override int GetHashCode()
    {
#pragma warning disable S3249
        return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
#pragma warning restore S2349
    }
}
