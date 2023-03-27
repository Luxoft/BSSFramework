namespace Framework.DomainDriven.BLL;

/// <summary>
/// Тип евентов объекта
/// </summary>
[Flags]
public enum EventRoleMode
{
    /// <summary>
    /// Сохранение
    /// </summary>
    Save = 1,

    /// <summary>
    /// Удаление
    /// </summary>
    Remove = 2,

    /// <summary>
    /// Прочие евенты
    /// </summary>
    Other = 4,

    /// <summary>
    /// Все возможные евенты
    /// </summary>
    ALL = Save + Remove + Other
}
