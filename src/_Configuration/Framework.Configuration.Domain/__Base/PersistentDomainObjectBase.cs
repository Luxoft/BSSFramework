using Framework.Persistent;
using Framework.DomainDriven.Serialization;
using Framework.Core;

namespace Framework.Configuration.Domain;

[TargetSystem("{50465868-4B49-42CF-A702-A39400E6C317}", "Configuration")]
public abstract class PersistentDomainObjectBase : DomainObjectBase, IEquatable<PersistentDomainObjectBase>, IIdentityObject<Guid>
{
    private Guid id;

    #region Constructor

    protected PersistentDomainObjectBase()
    {

    }

    protected PersistentDomainObjectBase(Guid id)
            : this ()
    {
        this.id = id;
    }

    #endregion

    /// <summary>
    /// ID доменного объекта
    /// </summary>
    public virtual Guid Id
    {
        get { return this.id; }
        protected internal set { this.id = value; }
    }

    /// <summary>
    /// Признак того, что класс еще не сохранен в базе
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual bool IsNew => this.Id == Guid.Empty;

    public static bool operator ==(PersistentDomainObjectBase a, PersistentDomainObjectBase b)
    {
        return object.ReferenceEquals(a, b) || (!object.ReferenceEquals(a, null) && !object.ReferenceEquals(b, null) && a.Equals(b));
    }

    public static bool operator !=(PersistentDomainObjectBase a, PersistentDomainObjectBase b)
    {
        return !(a == b);
    }

    public virtual bool Equals(PersistentDomainObjectBase obj)
    {
        return object.ReferenceEquals(this, obj) || (!object.ReferenceEquals(obj, null) && this.Id == obj.Id && this.Id != Guid.Empty);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as PersistentDomainObjectBase);
    }

    public override int GetHashCode()
    {
        return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
    }
}
