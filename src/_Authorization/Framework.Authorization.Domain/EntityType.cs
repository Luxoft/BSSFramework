using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.HierarchicalExpand;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.Domain;

/// <summary>
/// Описание доменных типов, в контексте которых выдаются права пользователю
/// </summary>
/// <remarks>
/// В коде контекст описывается следующими сущностями:
/// <seealso cref="EntityType"/>
/// <seealso cref="PermissionFilterEntity"/>
/// <seealso cref="PermissionFilterItem"/>
/// Типы, в контексте которых выдаются права пользователю, записываются вручную на уровне SQL в базу конкретной системы
/// </remarks>
[BLLViewRole]
public class EntityType : BaseDirectory, IEntityType<Guid>
{
    private readonly bool isFilter;

    private readonly bool expandable;

    /// <summary>
    /// Конструктор
    /// </summary>
    protected EntityType()
    {
    }

    public EntityType(bool isFilter, bool expandable)
    {
        this.isFilter = isFilter;
        this.expandable = expandable;
    }

    /// <summary>
    /// Признак того, что доменный тип является контекстом безопасности
    /// </summary>
    public virtual bool IsFilter => this.isFilter;

    /// <summary>
    /// Расширение прав по дереву в зависимости от типа Expand Type
    /// </summary>
    /// <seealso cref="HierarchicalExpandType"/>
    public virtual bool Expandable => this.expandable;

    /// <summary>
    /// Вычисляемое название доменного типа
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }
}
