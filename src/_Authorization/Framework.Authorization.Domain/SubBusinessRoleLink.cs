using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь между основной и дочерней ролью
/// </summary>
/// <remarks>
/// Роль <seealso cref="BusinessRole"/> – это набор секьюрных операций
/// Однако, для удобства поддержки иногда роль рассматривают как группу дочерних ролей <seealso cref="SubBusinessRole"/>, содержащих в себе набор секьюрных операций
/// Таким образом, роль является либо прямым агрегатором операций, либо агрегатором саб бизнес-ролей
/// </remarks>
public class SubBusinessRoleLink : AuditPersistentDomainObjectBase, IDetail<BusinessRole>
{
    private readonly BusinessRole businessRole;

    private BusinessRole subBusinessRole;

    protected SubBusinessRoleLink()
    {
    }

    /// <summary>
    /// Конструктор связи бизнес-роли и ее дочерней роли
    /// </summary>
    /// <param name="businessRole">Бизнес-роль</param>
    public SubBusinessRoleLink(BusinessRole businessRole)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.businessRole = businessRole;
        this.businessRole.AddDetail(this);
    }

    /// <summary>
    /// Конструктор связи бизнес-роли и ее дочерней роли
    /// </summary>
    /// <param name="businessRole">бизнес-роль</param>
    /// <param name="subBusinessRole">саб бизнес-роль</param>
    public SubBusinessRoleLink(BusinessRole businessRole, BusinessRole subBusinessRole)
            : this(businessRole)
    {
        this.subBusinessRole = subBusinessRole;
    }

    /// <summary>
    /// Бизнес-роль, к которой привязана саб бизнес-роль
    /// </summary>
    [IsMaster]
    public virtual BusinessRole BusinessRole
    {
        get { return this.businessRole; }
    }

    /// <summary>
    /// Саб бизнес-роль, к которой привязана бизнес-роль
    /// </summary>
    [UniqueElement]
    [Required]
    [FixedPropertyValidator]
    public virtual BusinessRole SubBusinessRole
    {
        get { return this.subBusinessRole; }
        set { this.subBusinessRole = value; }
    }

    BusinessRole Persistent.IDetail<BusinessRole>.Master
    {
        get { return this.businessRole; }
    }
}
