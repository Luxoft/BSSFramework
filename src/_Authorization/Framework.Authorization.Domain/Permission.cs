using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Право доступа принципала на выполнение определенных действий в системе
/// </summary>
/// <remarks>
/// Пермиссии могут выдаваться в рамках контекстов
/// </remarks>
/// <seealso cref="SecurityContextType"/>
[IgnoreHbmMapping]
[System.Diagnostics.DebuggerDisplay("Principal={Principal.Name}, Role={Role.Name}")]
public class Permission : AuditPersistentDomainObjectBase,

                          IDetail<Principal>,

                          IMaster<PermissionRestriction>,

                          IMaster<Permission>,

                          IDetail<Permission>,

                          IDefaultHierarchicalPersistentDomainObjectBase<Permission>,

                          IPeriodObject
{
    private readonly ICollection<PermissionRestriction> restrictions = new List<PermissionRestriction>();

    private readonly ICollection<Permission> delegatedTo = new List<Permission>();

    private readonly Principal principal;

    private readonly Permission delegatedFrom;

    private BusinessRole role;

    private Period period = Period.Eternity;

    private string comment;

    protected Permission()
    {
    }

    /// <summary>
    /// Конструктор создает пермиссию с ссылкой на принципала
    /// </summary>
    /// <param name="principal">принципал</param>
    public Permission(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        this.principal = principal;
        this.principal.AddDetail(this);
    }

    /// <summary>
    /// Конструктор создает делегированную пермиссию для принципала
    /// </summary>
    /// <param name="principal">Принципал</param>
    /// <param name="delegatedFrom">Пермиссия, от которой происходит делегирование</param>
    public Permission(Principal principal, Permission delegatedFrom)
        : this(principal)
    {
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        this.delegatedFrom = delegatedFrom;
        this.delegatedFrom.AddDetail(this);
    }

    /// <summary>
    /// Коллекция контекстов пермиссии
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<PermissionRestriction> Restrictions => this.restrictions;

    /// <summary>
    /// Коллекция пермиссий, которым данная пермиссия была делегирована
    /// </summary>
    [Mapping(CascadeMode = CascadeMode.Enabled)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual IEnumerable<Permission> DelegatedTo => this.delegatedTo;

    /// <summary>
    /// Пермиссия, от которой была делегирована данная пермиссия
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Permission DelegatedFrom => this.delegatedFrom;

    /// <summary>
    /// Период действия пермиссии
    /// </summary>
    public virtual Period Period { get => this.period.Round(); set => this.period = value.Round(); }

    /// <summary>
    /// Приниципал, к которому относится данная пермиссия
    /// </summary>
    public virtual Principal Principal => this.principal;

    /// <summary>
    /// Бизнес-роль, которую содержит пермиссия
    /// </summary>
    [Required]
    [FixedPropertyValidator]
    public virtual BusinessRole Role { get => this.role; set => this.role = value; }

    /// <summary>
    /// Комментарий к пермиссии
    /// </summary>
    [MaxLength]
    public virtual string Comment { get => this.comment.TrimNull(); set => this.comment = value.TrimNull(); }

    ICollection<PermissionRestriction> IMaster<PermissionRestriction>.Details => (ICollection<PermissionRestriction>)this.Restrictions;

    Principal IDetail<Principal>.Master => this.Principal;

    ICollection<Permission> IMaster<Permission>.Details => (ICollection<Permission>)this.DelegatedTo;

    Permission IDetail<Permission>.Master => this.DelegatedFrom;

    Permission IParentSource<Permission>.Parent => this.DelegatedFrom;

    IEnumerable<Permission> IChildrenSource<Permission>.Children => this.DelegatedTo;
}
