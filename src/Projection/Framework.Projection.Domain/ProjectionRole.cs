namespace Framework.Projection;

/// <summary>
/// Роль проекции
/// </summary>
public enum ProjectionRole
{
    /// <summary>
    /// Обычная проекция
    /// </summary>
    Default,

    /// <summary>
    /// Автоматически достраиваемая проекция (для переходов через ExpandPath)
    /// </summary>
    AutoNode,

    /// <summary>
    /// Проекция для безопасности
    /// </summary>
    SecurityNode
}
