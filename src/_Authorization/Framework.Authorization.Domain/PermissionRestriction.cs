using Framework.BLL.Domain.MasterDetails;
using Framework.BLL.Domain.Serialization;
using Framework.Restriction;
using Framework.Tracking.Validation.BLL.Validation.Attributes;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь между пермиссией и контекстом
/// </summary>
public class PermissionRestriction : AuditPersistentDomainObjectBase, IDetail<Permission>
{
    private Permission permission;

    private SecurityContextType securityContextType;

    private Guid securityContextId;

    public PermissionRestriction()
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
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Permission Permission
    {
        get { return this.permission; }
        set { this.permission = value; }
    }

    /// <summary>
    /// Вычисляемый доменный тип по связке контекст+пермиссия
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    [FixedPropertyValidator]
    [Required]
    [UniqueElement]
    public virtual SecurityContextType SecurityContextType
    {
        get => this.securityContextType;
        set => this.securityContextType = value;
    }

    [FixedPropertyValidator]
    [Required]
    [UniqueElement]
    public virtual Guid SecurityContextId { get => this.securityContextId; set => this.securityContextId = value; }

    Permission IDetail<Permission>.Master => this.Permission;
}
