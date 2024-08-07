using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Persistent;
using Framework.Restriction;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь между пермиссией и контекстом
/// </summary>
public class PermissionRestriction : AuditPersistentDomainObjectBase, IDetail<Permission>, IPermissionRestriction<Guid>
{
    private readonly Permission permission;

    private SecurityContextType securityContextType;

    private Guid securityContextId;

    protected PermissionRestriction()
    {
    }

    /// <summary>
    /// Конструктор создает контекст с ссылкой на пермиссию
    /// </summary>
    /// <param name="permission">Пермиссия</param>
    public PermissionRestriction(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        this.permission = permission;
        this.permission.AddDetail(this);
    }

    /// <summary>
    /// Пермиссия по связке контекст+пермиссия
    /// </summary>
    public virtual Permission Permission => this.permission;

    /// <summary>
    /// Вычисляемый доменный тип по связке контекст+пермиссия
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    [FixedPropertyValidator]
    [Required]
    [UniqueElement]
    public virtual SecurityContextType SecurityContextType
    {
        get { return this.securityContextType; }
        set { this.securityContextType = value; }
    }

    [FixedPropertyValidator]
    [Required]
    [UniqueElement]
    public virtual Guid SecurityContextId
    {
        get { return this.securityContextId; }
        set { this.securityContextId = value; }
    }

    [ExpandPath("SecurityContextType.Id")]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual Guid SecurityContextTypeId => this.SecurityContextType.Id;

    Permission IDetail<Permission>.Master => this.Permission;
}
