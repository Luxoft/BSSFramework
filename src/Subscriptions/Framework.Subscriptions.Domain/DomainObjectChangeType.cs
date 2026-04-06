namespace Framework.Subscriptions.Domain;

/// <summary>
/// Описывает тип изменения доменного объекта для которого должна срабатывать подписка.
/// </summary>
[Flags]
public enum DomainObjectChangeType
{
    /// <summary>
    /// Создание доменного объекта.
    /// </summary>
    Create = 1,

    /// <summary>
    /// Изменение доменного объекта.
    /// </summary>
    Update = 2,

    /// <summary>
    /// Удаление доменного объекта.
    /// </summary>
    Delete = 4,

    /// <summary>
    /// Любое изменение.
    /// </summary>
    Any = Create | Update | Delete
}
