namespace Framework.Application.Domain.Attributes;

/// <summary>
/// Признак, является ли свойство деталью объекта
/// Если свойство - деталь, то она сохраняется в базу данных вместе с сохранением master-объекта,
/// а также в Rich DTO оно тоже Rich(соответственно из Full DTO оно пропадает).
///
/// Если коллекция помечена атрибутом DetailRole(false) то она не является деталью объекта и не сохраняется в бд при изменении master-объекта.
///
/// <see href="confluence/display/IADFRAME/DetailRoleAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Class)]
public class DetailRoleAttribute(DetailRole role) : Attribute
{
    public DetailRoleAttribute(bool yesOrNo)
            : this(yesOrNo ? DetailRole.Yes : DetailRole.No)
    {
    }

    public DetailRole Role { get; } = role;

    public bool HasValue(bool value) => value ? this.Role == DetailRole.Yes : this.Role == DetailRole.No;
}
