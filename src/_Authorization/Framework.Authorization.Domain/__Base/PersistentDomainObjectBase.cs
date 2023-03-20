using System;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

[TargetSystem("{f065289e-4dc5-48c9-be44-a2ee0131e631}")]
public abstract class PersistentDomainObjectBase : DomainObjectBase, IEquatable<PersistentDomainObjectBase>, IIdentityObject<Guid>
{
    private Guid id;

    protected PersistentDomainObjectBase()
    {
    }

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
#pragma warning disable S3249
        return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
#pragma warning restore S2349
    }
}
