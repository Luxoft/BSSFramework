namespace Framework.Events;

/// <summary>
/// Константы, описывающие тип события(event-a во внешнюю систему)
/// </summary>
public record EventOperation(string Name)
{
    /// <summary>
    /// Сохранение объекта
    /// </summary>
    public static EventOperation Save { get; } = new(nameof(Save));

    /// <summary>
    /// Удаление объекта
    /// </summary>
    public static EventOperation Remove { get; } = new(nameof(Remove));
}
