using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;

namespace Framework.Authorization.Domain;

/// <summary>
/// Описание действия в системе
/// </summary>
/// <remarks>
/// Операцию бывают:
/// Контекстные - право доступа к объекту относительно контекста
/// Неконтекстные - право доступа к объекту без учета контекста
/// </remarks>
[DomainType("{34ef2920-d044-4b58-9928-a2ee0131e635}")]
[ViewDomainObject(typeof(AuthorizationSecurityOperation), nameof(AuthorizationSecurityOperation.OperationView))]
[EditDomainObject(typeof(AuthorizationSecurityOperation), nameof(AuthorizationSecurityOperation.OperationEdit))]
[BLLViewRole]
[BLLSaveRole(AllowCreate = false)]
[UniqueGroup]
public class Operation : BaseDirectory, IParentSource<Operation>
{
    private readonly ICollection<BusinessRoleOperationLink> links = new List<BusinessRoleOperationLink>();

    private string description;

    private Operation approveOperation;

    /// <summary>
    /// Коллекция связей операции с ролью
    /// </summary>
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<BusinessRoleOperationLink> Links
    {
        get { return this.links; }
    }

    /// <summary>
    /// Описание операции
    /// </summary>
    public virtual string Description
    {
        get { return this.description.TrimNull(); }
        set { this.description = value.TrimNull(); }
    }

    /// <summary>
    /// Секьюрная операция, подтверждение по которой нужно получить от уполномоченных лиц
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Operation ApproveOperation
    {
        get { return this.approveOperation; }
        set { this.approveOperation = value; }
    }

    /// <summary>
    /// Вычисляемое имя операции
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }

    Operation IParentSource<Operation>.Parent
    {
        get { return this.ApproveOperation; }
    }
}
