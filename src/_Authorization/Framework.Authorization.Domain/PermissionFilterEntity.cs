using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Restriction;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Описание доменных объектов, в контексте которых выдаются права пользователю
    /// </summary>
    [ViewDomainObject]
    [UniqueGroup]
    [BLLRole]
    public class PermissionFilterEntity : AuditPersistentDomainObjectBase, IPermissionFilterEntity<Guid>
    {
        private EntityType entityType;

        private Guid entityId;

        /// <summary>
        /// Тип доменного объекта
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual EntityType EntityType
        {
            get { return this.entityType; }
            set { this.entityType = value; }
        }

        /// <summary>
        /// ID доменного объекта
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual Guid EntityId
        {
            get { return this.entityId; }
            set { this.entityId = value; }
        }

        IEntityType IPermissionFilterEntity<Guid>.EntityType
        {
            get { return this.EntityType; }
        }
    }
}
