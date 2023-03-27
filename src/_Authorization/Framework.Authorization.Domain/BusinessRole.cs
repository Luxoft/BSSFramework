using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

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

    public const string AdminRoleName = "Administrator";

    [UniqueGroup]
    public virtual IEnumerable<BusinessRoleOperationLink> BusinessRoleOperationLinks => this.businessRoleOperationLinks;

    /// <summary>
    /// Коллекция связей бизнес-роли с дочерними ролями
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<SubBusinessRoleLink> SubBusinessRoleLinks => this.subBusinessRoleLinks;

    /// <summary>
    /// Коллекция пермиссий принципалов, выданных по одной бизнес-роль
    /// </summary>
    [DetailRole(false)]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<Permission> Permissions => this.permissions;

    /// <summary>
    /// Вычисляемая коллекция дочерних ролей, выданных на одну бизнес-роль
    /// </summary>
    [DetailRole(false)]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<BusinessRole> SubBusinessRoles => this.SubBusinessRoleLinks.Select(link => link.SubBusinessRole);

    /// <summary>
    /// Описание бизнес-роли
    /// </summary>
    public virtual string Description
    {
        get => this.description.TrimNull();
        set => this.description = value.TrimNull();
    }

    /// <summary>
    /// Вычисляемый признак того, что текущая бизнес-роль является админской
    /// </summary>
    public virtual bool IsAdmin => this.Name == AdminRoleName;

    /// <summary>
    /// Вычисляемый признак необходимости подтверждения выдачи бизнес-роли
    /// </summary>
    /// <remarks>
    /// Если в роль входит хотя бы одна операция "ApproveOperation", то она должна быть утверждена уполномоченными лицами
    /// </remarks>
    public virtual bool RequiredApprove => this.BusinessRoleOperationLinks.Any(link => link.Operation.ApproveOperation != null);

    ICollection<BusinessRoleOperationLink> IMaster<BusinessRoleOperationLink>.Details =>
            (ICollection<BusinessRoleOperationLink>)this.BusinessRoleOperationLinks;

    ICollection<SubBusinessRoleLink> IMaster<SubBusinessRoleLink>.Details =>
            (ICollection<SubBusinessRoleLink>)this.SubBusinessRoleLinks;

    IEnumerable<BusinessRole> IChildrenSource<BusinessRole>.Children => this.SubBusinessRoles;
}
