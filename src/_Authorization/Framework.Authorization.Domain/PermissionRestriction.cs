using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Persistent;
using Framework.Restriction;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь между пермиссией и контекстом
/// </summary>
[DomainType("{48880DB2-1BC0-4130-BC87-F0E8E0D246CC}")]
[BLLRole]
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
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    [UniqueElement]
    public virtual SecurityContextType SecurityContextType
    {
        get { return this.securityContextType; }
        set { this.securityContextType = value; }
    }

    [FixedPropertyValidator]
    [Required]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    [UniqueElement]
    public virtual Guid SecurityContextId
    {
        get { return this.securityContextId; }
        set { this.securityContextId = value; }
    }

    Permission IDetail<Permission>.Master => this.Permission;

    ISecurityContextType<Guid> IPermissionRestriction<Guid>.SecurityContextType => this.SecurityContextType;
}
