namespace Framework.Subscriptions.Domain;

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
public interface IDomainObjectVersions
{
    object? Previous { get; }

    object? Current { get; }
}
