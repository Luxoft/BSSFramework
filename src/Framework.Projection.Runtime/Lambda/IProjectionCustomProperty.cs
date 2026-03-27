namespace Framework.Projection.Lambda;

/// <summary>
/// Кастомное проекционное свойство
/// </summary>
public interface IProjectionCustomProperty : IProjectionAttributeProvider
{
    /// <summary>
    /// Имя свойства
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Доступна запись
    /// </summary>
    bool Writable { get; }

    /// <summary>
    /// Тип свойства
    /// </summary>
    TypeReferenceBase Type { get; }

    /// <summary>
    /// Дополнительная подгрузка свойств
    /// </summary>
    IReadOnlyList<string> Fetchs { get; }
}
