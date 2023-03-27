namespace Framework.Projection.Lambda;

/// <summary>
/// Объект с дополнительными атрибутами для генерации
/// </summary>
public interface IProjectionAttributeProvider
{
    /// <summary>
    /// Дополнительные атрибуты генерации
    /// </summary>
    IReadOnlyList<Attribute> Attributes { get; }
}
