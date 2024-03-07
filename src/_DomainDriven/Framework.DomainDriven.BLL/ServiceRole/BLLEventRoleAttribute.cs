namespace Framework.DomainDriven.BLL;

/// <summary>
/// Маркер объектов отправляющих евенты в интеграцию
/// </summary>
public class BLLEventRoleAttribute : Attribute
{
    /// <summary>
    /// Фильтрация оправляемых евентов
    /// </summary>
    public EventRoleMode Mode { get; set; } = EventRoleMode.ALL;
}
