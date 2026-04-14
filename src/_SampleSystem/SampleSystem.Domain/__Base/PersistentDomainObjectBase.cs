using Framework.Application.Domain;
using Framework.BLL.Domain.Serialization;
using Framework.Core;

// ReSharper disable once CheckNamespace
namespace SampleSystem.Domain;

public abstract class PersistentDomainObjectBase : DomainObjectBase, IIdentityObject<Guid>, IEquatable<PersistentDomainObjectBase>
{
    private Guid id;

    #region Constructor

    protected PersistentDomainObjectBase()
    {

    }

    #endregion

    /// <summary>
    /// ID доменного объекта
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Guid Id
    {
        get => this.id;
        set => this.id = value;
    }

    /// <summary>
    /// Признак того, что класс еще не сохранен в базе
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual bool IsNew => this.Id == Guid.Empty;

    public static bool operator ==(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => ReferenceEquals(a, b) || (!ReferenceEquals(a, null) && !ReferenceEquals(b, null) && a.Equals(b));

    public static bool operator !=(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => !(a == b);

    public virtual bool Equals(PersistentDomainObjectBase? obj) =>
        ReferenceEquals(this, obj)
        || (!ReferenceEquals(obj, null)
            && this.Id == obj.Id
            && this.Id != Guid.Empty

            && (obj.GetType().IsAssignableFrom(this.GetType()) || this.GetType().IsAssignableFrom(obj.GetType()))
           );

    public override bool Equals(object? obj) => this.Equals(obj as PersistentDomainObjectBase);

    public override int GetHashCode() => this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
}
