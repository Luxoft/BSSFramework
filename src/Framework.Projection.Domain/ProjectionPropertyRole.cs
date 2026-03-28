namespace Framework.Projection;

/// <summary>
/// Роль свойства проекции
/// </summary>
public enum ProjectionPropertyRole
{
    /// <summary>
    /// Обычное свойство
    /// </summary>
    Default,

    /// <summary>
    /// Автоматически достраиваемое свойство в цепочке переходов
    /// </summary>
    AutoNode,

    /// <summary>
    /// Последнее автоматически достраиваемое свойство в цепочке переходов
    /// </summary>
    LastAutoNode,

    /// <summary>
    /// Автоматически достраиваемое Parent-свойство во внутренней коллекции
    /// </summary>
    MissedParent,

    /// <summary>
    /// Автоматически достраиваемое свойства для безопасности
    /// </summary>
    Security,

    /// <summary>
    /// Расчётное свойство
    /// </summary>
    Custom
}
