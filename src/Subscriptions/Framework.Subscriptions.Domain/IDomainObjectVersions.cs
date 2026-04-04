namespace Framework.Subscriptions.Domain;

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
public interface IDomainObjectVersions
{
    Type DomainObjectType { get; }

    DomainObjectChangeType ChangeType { get; }

    object? Previous { get; }

    object? Current { get; }
}
