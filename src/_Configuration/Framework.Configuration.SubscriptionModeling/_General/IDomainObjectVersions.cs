namespace Framework.Configuration.SubscriptionModeling._General;

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
public interface IDomainObjectVersions
{
    object? Previous { get; }

    object? Current { get; }
}
