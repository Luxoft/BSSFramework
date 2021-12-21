using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Набор секьюрных операций, который выдается принципалу вместе с контекcтом их применения
    /// </summary>
    [DomainType("{3823172C-B703-46FD-A82F-B55833EBCD38}")]
    [UniqueGroup]
    [AuthorizationViewDomainObject(AuthorizationSecurityOperationCode.BusinessRoleView)]
    [AuthorizationEditDomainObject(AuthorizationSecurityOperationCode.BusinessRoleEdit)]
    [BLLViewRole]
    [BLLSaveRole]
    [BLLRemoveRole]
    public class BusinessRole : BaseDirectory,
        IMaster<BusinessRoleOperationLink>,
        IMaster<SubBusinessRoleLink>,
        IChildrenSource<BusinessRole>
    {
        private readonly ICollection<BusinessRoleOperationLink> businessRoleOperationLinks = new List<BusinessRoleOperationLink>();

        private readonly ICollection<Permission> permissions = new List<Permission>();

        private readonly ICollection<SubBusinessRoleLink> subBusinessRoleLinks = new List<SubBusinessRoleLink>();

        private string description;

        /// <summary>
        /// Название администраторской роли
        /// </summary>
        public const string AdminRoleName = "Administrator";

        /// <summary>
        /// Конструктор
        /// </summary>
        public BusinessRole()
        {
        }

        /// <summary>
        /// Коллекция связей бизнес-роли с операциями
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<BusinessRoleOperationLink> BusinessRoleOperationLinks
        {
            get { return this.businessRoleOperationLinks; }
        }

        /// <summary>
        /// Коллекция связей бизнес-роли с дочерними ролями
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<SubBusinessRoleLink> SubBusinessRoleLinks
        {
            get { return this.subBusinessRoleLinks; }
        }

        /// <summary>
        /// Коллекция пермиссий принципалов, выданных по одной бизнес-роль
        /// </summary>
        [DetailRole(false)]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<Permission> Permissions
        {
            get { return this.permissions; }
        }

        /// <summary>
        /// Вычисляемая коллекция дочерних ролей, выданных на одну бизнес-роль
        /// </summary>
        [DetailRole(false)]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<BusinessRole> SubBusinessRoles
        {
            get { return this.SubBusinessRoleLinks.Select(link => link.SubBusinessRole); }
        }

        /// <summary>
        /// Вычисляемое название бизнес-роли
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set { this.SetValueSafe(v => v.Name, value, () => base.Name = value); }
        }

        /// <summary>
        /// Описание бизнес-роли
        /// </summary>
        public virtual string Description
        {
            get { return this.description.TrimNull(); }
            set { this.description = value.TrimNull(); }
        }

        /// <summary>
        /// Вычисляемый признак того, что текущая бизнес-роль является админской
        /// </summary>
        public virtual bool IsAdmin
        {
            get { return this.Name == AdminRoleName; }
        }

        /// <summary>
        /// Вычисляемый признак необходимости подтверждения выдачи бизнес-роли
        /// </summary>
        /// <remarks>
        /// Если в роль входит хотя бы одна операция "ApproveOperation", то она должна быть утверждена уполномоченными лицами
        /// </remarks>
        public virtual bool RequiredApprove
        {
            get { return this.BusinessRoleOperationLinks.Any(link => link.Operation.ApproveOperation != null); }
        }

        /// <summary>
        /// Коллекция связей операции с бизнес-ролью
        /// </summary>
        ICollection<BusinessRoleOperationLink> IMaster<BusinessRoleOperationLink>.Details
        {
            get { return (ICollection<BusinessRoleOperationLink>)this.BusinessRoleOperationLinks; }
        }

        /// <summary>
        /// Коллекция связей дочерней роли с бизнес-ролью
        /// </summary>
        ICollection<SubBusinessRoleLink> IMaster<SubBusinessRoleLink>.Details
        {
            get { return (ICollection<SubBusinessRoleLink>)this.SubBusinessRoleLinks; }
        }

        IEnumerable<BusinessRole> IChildrenSource<BusinessRole>.Children
        {
            get { return this.SubBusinessRoles; }
        }
    }
}
