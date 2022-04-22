using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.SecuritySystem;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Право доступа принципала на выполнение определенных действий в системе
    /// </summary>
    /// <remarks>
    /// Пермиссии могут выдаваться в рамках контекстов
    /// </remarks>
    /// <seealso cref="EntityType"/>
    [DomainType("{5d774041-bc69-4841-b64e-a2ee0131e632}")]
    [AuthorizationViewDomainObject(typeof(Principal))]
    [AuthorizationEditDomainObject(typeof(Principal))]
    [BLLViewRole(MaxCollection = MainDTOType.RichDTO)]
    [BLLRemoveRole]
    [System.Diagnostics.DebuggerDisplay("Principal={Principal.Name}, Role={Role.Name}")]
    public class Permission : AuditPersistentDomainObjectBase,

        IDetail<Principal>,

        IMaster<PermissionFilterItem>,

        IMaster<Permission>,

        IMaster<DenormalizedPermissionItem>,

        IDetail<Permission>,

        IDefaultHierarchicalPersistentDomainObjectBase<Permission>,

        IPeriodObject,

        IPermission<Guid>,

        IStatusObject<PermissionStatus>
    {
        private readonly ICollection<PermissionFilterItem> filterItems = new List<PermissionFilterItem>();

        private readonly ICollection<DenormalizedPermissionItem> denormalizedItems = new List<DenormalizedPermissionItem>();

        private readonly ICollection<Permission> delegatedTo = new List<Permission>();

        private readonly Principal principal;

        private readonly Permission delegatedFrom;

        private readonly bool isDelegatedFrom;

        private BusinessRole role;

        private bool isDelegatedTo;

        private Period period = Period.Eternity;

        private PermissionStatus status = PermissionStatus.Approved;

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

            this.isDelegatedFrom = true;
        }

        /// <summary>
        /// Коллекция контекстов пермиссии
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<PermissionFilterItem> FilterItems => this.filterItems;

        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<DenormalizedPermissionItem> DenormalizedItems => this.denormalizedItems;

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
        public virtual Period Period
        {
            get { return this.period.Round(); }
            set { this.period = value.Round(); }
        }

        /// <summary>
        /// Статус пермиссии
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual PermissionStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary>
        /// Признак того, что даннная пермиссия была делегирована кому-либо
        /// </summary>
        public virtual bool IsDelegatedTo
        {
            get { return this.isDelegatedTo; }
            internal protected set { this.isDelegatedTo = value; }
        }

        /// <summary>
        /// Признак того, что данная пермиссия была делегирована от кого-то
        /// </summary>
        public virtual bool IsDelegatedFrom => this.isDelegatedFrom;

        /// <summary>
        /// Вычисляемый принципал, который делегировал пермиссию
        /// </summary>
        [ExpandPath("DelegatedFrom.Principal")]
        public virtual Principal DelegatedFromPrincipal
        {
            get { return this.DelegatedFrom.Maybe(v => v.Principal); }
        }

        /// <summary>
        /// Приниципал, к которому относится данная пермиссия
        /// </summary>
        public virtual Principal Principal
        {
            get { return this.principal; }
        }

        /// <summary>
        /// Бизнес-роль, которую содержит пермиссия
        /// </summary>
        [Required]
        public virtual BusinessRole Role
        {
            get { return this.role; }
            set { this.SetValueSafe(v => v.role, value); }
        }

        /// <summary>
        /// Комментарий к пермиссии
        /// </summary>
        [MaxLength]
        public virtual string Comment
        {
            get { return this.comment.TrimNull(); }
            set { this.comment = value.TrimNull(); }
        }

        ICollection<PermissionFilterItem> IMaster<PermissionFilterItem>.Details => (ICollection<PermissionFilterItem>)this.FilterItems;

        ICollection<DenormalizedPermissionItem> IMaster<DenormalizedPermissionItem>.Details => (ICollection<DenormalizedPermissionItem>)this.DenormalizedItems;

        Principal IDetail<Principal>.Master => this.Principal;

        ICollection<Permission> IMaster<Permission>.Details => (ICollection<Permission>)this.DelegatedTo;

        Permission IDetail<Permission>.Master => this.DelegatedFrom;

        Permission IParentSource<Permission>.Parent => this.DelegatedFrom;

        IEnumerable<Permission> IChildrenSource<Permission>.Children => this.DelegatedTo;

        IEnumerable<IPermissionFilterItem<Guid>> IPermission<Guid>.FilterItems => this.FilterItems;

        /// <summary>
        /// Проверка на уникальноть
        /// </summary>
        /// <param name="otherPermission">Другая пермиссия</param>
        /// <returns></returns>
        public virtual bool IsDuplicate([NotNull] Permission otherPermission)
        {
            if (otherPermission == null) throw new ArgumentNullException(nameof(otherPermission));

            return otherPermission.Role == this.Role
                   && otherPermission.Period.IsIntersected(this.Period)
                   && otherPermission.GetOrderedEntityIdents().SequenceEqual(this.GetOrderedEntityIdents());
        }
    }
}
