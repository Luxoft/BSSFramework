using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Связь операции и бизнес-роли
/// </summary>
public class BusinessRoleOperationLink : AuditPersistentDomainObjectBase, IDetail<BusinessRole>
{
    private readonly BusinessRole businessRole;

    private Operation operation;

    private bool isDenormalized;

    protected BusinessRoleOperationLink()
    {
    }

    /// <summary>
    /// Конструктор создает связь между бизнес-ролью и операцией
    /// </summary>
    /// <param name="businessRole">Бизнес-роль</param>
    public BusinessRoleOperationLink(BusinessRole businessRole)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.businessRole = businessRole;
        this.businessRole.AddDetail(this);
    }

    /// <summary>
    /// Бизнес-роль, к которой привязана операция
    /// </summary>
    public virtual BusinessRole BusinessRole
    {
        get { return this.businessRole; }
    }

    /// <summary>
    /// Операция, к которой привязана бизнес-роль
    /// </summary>
    [UniqueElement]
    [Required]
    [FixedPropertyValidator]
    public virtual Operation Operation
    {
        get { return this.operation; }
        set { this.operation = value; }
    }

    /// <summary>
    /// Признак денормализации
    /// </summary>
    /// <remarks>
    /// Признак "Is Denormalized" позволяет ускорить поиск по дереву, путем добавления изыточностых данных
    /// Данные в Sub Business Roles нормализированы, данные в Business Roles - денормализизованы
    /// </remarks>
    public virtual bool IsDenormalized
    {
        get { return this.isDenormalized; }
        internal protected set { this.isDenormalized = value; }
    }

    BusinessRole IDetail<BusinessRole>.Master
    {
        get { return this.BusinessRole; }
    }
}
