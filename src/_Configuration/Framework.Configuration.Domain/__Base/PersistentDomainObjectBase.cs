using Framework.Application.Domain;
using Framework.Core;

namespace Framework.Configuration.Domain;

public abstract class PersistentDomainObjectBase : DomainObjectBase, IEquatable<PersistentDomainObjectBase>, IIdentityObject<Guid>
{
    private Guid id;

    #region Constructor

    protected PersistentDomainObjectBase()
    {

    }

    protected PersistentDomainObjectBase(Guid id)
            : this () =>
        this.id = id;

    #endregion

    /// <summary>
    /// ID доменного объекта
    /// </summary>
    public virtual Guid Id
    {
        get => this.id;
        set => this.id = value;
    }

    /// <summary>
    /// Признак того, что класс еще не сохранен в базе
    /// </summary>
    public virtual bool IsNew => this.Id == Guid.Empty;

    public static bool operator ==(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => ReferenceEquals(a, b) || (!ReferenceEquals(a, null) && !ReferenceEquals(b, null) && a.Equals(b));

    public static bool operator !=(PersistentDomainObjectBase? a, PersistentDomainObjectBase? b) => !(a == b);

    public virtual bool Equals(PersistentDomainObjectBase? obj) => ReferenceEquals(this, obj) || (!ReferenceEquals(obj, null) && this.Id == obj.Id && this.Id != Guid.Empty);

    public override bool Equals(object? obj) => this.Equals(obj as PersistentDomainObjectBase);

    public override int GetHashCode() => this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
}
