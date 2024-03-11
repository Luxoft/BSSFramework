namespace Framework.Events;

/// <summary>
/// Константы, описывающие тип события(event-a во внешнюю систему)
/// </summary>
public record DomainObjectEvent(string Name)
{
    /// <summary>
    /// Сохранение объекта
    /// </summary>
    public static DomainObjectEvent Save { get; } = new(nameof(Save));

    /// <summary>
    /// Удаление объекта
    /// </summary>
    public static DomainObjectEvent Remove { get; } = new(nameof(Remove));

    public sealed override string ToString() => this.Name;
}
