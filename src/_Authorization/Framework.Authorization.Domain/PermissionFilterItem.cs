using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Связь между пермиссией и контекстом
    /// </summary>
    [DomainType("{48880DB2-1BC0-4130-BC87-F0E8E0D246CC}")]
    [ViewDomainObject]
    [BLLRole]
    public partial class PermissionFilterItem : AuditPersistentDomainObjectBase, IDetail<Permission>, IPermissionFilterItem<Guid>
    {
        private readonly Permission permission;

        private readonly PermissionFilterEntity entity;

        protected PermissionFilterItem()
        {
        }

        /// <summary>
        /// Конструктор создает контекст с ссылкой на пермиссию
        /// </summary>
        /// <param name="permission">Пермиссия</param>
        public PermissionFilterItem(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            this.permission = permission;
            this.permission.AddDetail(this);
        }

        /// <summary>
        /// Конструктор создает FilterItem для конкретной пермиссии с указанием FilterEntity
        /// </summary>
        /// <param name="permission">Пермиссия</param>
        /// <param name="entity">Контекст</param>
        public PermissionFilterItem(Permission permission, PermissionFilterEntity entity)
            : this(permission)
        {
            this.entity = entity;
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
        public virtual PermissionFilterEntity Entity
        {
            get { return this.entity; }
            set { this.SetValueSafe(v => v.entity, value); }
        }

        /// <summary>
        /// Вычисляемый доменный тип по связке контекст+пермиссия
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
        [ExpandPath("Entity.EntityType")]
        public virtual EntityType EntityType
        {
            get { return this.Entity.EntityType; }
        }

        Permission IDetail<Permission>.Master
        {
            get { return this.permission; }
        }

        IPermissionFilterEntity<Guid> IPermissionFilterItem<Guid>.Entity
        {
            get { return this.Entity; }
        }
    }
}
