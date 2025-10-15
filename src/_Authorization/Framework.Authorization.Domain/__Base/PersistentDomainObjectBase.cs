using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

public abstract class PersistentDomainObjectBase : DomainObjectBase, IEquatable<PersistentDomainObjectBase>, IIdentityObject<Guid>
{
    private Guid id;

    protected PersistentDomainObjectBase()
    {
    }

    /// <summary>
    /// ID доменного объекта
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Guid Id
    {
        get { return this.id; }
        set { this.id = value; }
    }

    /// <summary>
    /// Признак того, что класс еще не сохранен в базе
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual bool IsNew => this.Id == Guid.Empty;

    public static bool operator ==(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b)
    {
        return ReferenceEquals(a, b) || (!ReferenceEquals(a, null) && !ReferenceEquals(b, null) && a.Equals(b));
    }

    public static bool operator !=(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b)
    {
        return !(a == b);
    }

    public virtual bool Equals(PersistentDomainObjectBase? obj)
    {
        return ReferenceEquals(this, obj) || (!ReferenceEquals(obj, null) && this.Id == obj.Id && this.Id != Guid.Empty);
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as PersistentDomainObjectBase);
    }

    public override int GetHashCode()
    {
#pragma warning disable S3249
        return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
#pragma warning restore S2349
    }
}
