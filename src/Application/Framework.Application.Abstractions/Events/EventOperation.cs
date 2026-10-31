namespace Framework.Application.Events;

/// <summary>
/// Constants describing the event type (event to an external system)
/// </summary>
public record EventOperation(string Name)
{
    /// <summary>
    /// Saving an object
    /// </summary>
    public static EventOperation Save { get; } = new(nameof(Save));

    /// <summary>
    /// Deleting an object
    /// </summary>
    public static EventOperation Remove { get; } = new(nameof(Remove));

    public sealed override string ToString() => this.Name;
}
