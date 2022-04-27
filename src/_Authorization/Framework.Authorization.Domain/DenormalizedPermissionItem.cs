using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь между пермиссией и контекстом
/// </summary>
[DomainType("{81CFA0DD-4190-40B7-8D08-3ABCA08BCABE}")]
[BLLRole]
public class DenormalizedPermissionItem : AuditPersistentDomainObjectBase, IDetail<Permission>, IDenormalizedPermissionItem<Guid>
{
    private readonly Permission permission;

    private readonly EntityType entityType;

    private Guid entityId;

    public static readonly Guid GrandAccessGuid = new Guid("{77777777-7777-7777-7777-777777777777}");

    protected DenormalizedPermissionItem()
    {
    }

    /// <summary>
    /// Конструктор создает контекст с ссылкой на пермиссию
    /// </summary>
    /// <param name="permission">Пермиссия</param>
    public DenormalizedPermissionItem(Permission permission, [NotNull] EntityType entityType, Guid entityId)
    {
        this.permission = permission ?? throw new ArgumentNullException(nameof(permission));
        this.entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        this.entityId = entityId;

        this.permission.AddDetail(this);
    }

    /// <summary>
    /// Пермиссия по связке контекст+пермиссия
    /// </summary>
    public virtual Permission Permission
    {
        get { return this.permission; }
    }

    /// <summary>
    /// Доменный объект по связке контекст+пермиссия
    /// </summary>
    [UniqueElement]
    [Required]
    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client)]
    public virtual EntityType EntityType
    {
        get { return this.entityType; }
    }

    [UniqueElement]
    public virtual Guid EntityId
    {
        get { return this.entityId; }
    }

    Permission IDetail<Permission>.Master
    {
        get { return this.permission; }
    }

    IEntityType<Guid> IDenormalizedPermissionItem<Guid>.EntityType => this.EntityType;
}
