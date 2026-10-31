namespace Framework.Subscriptions.Domain;

/// <summary>
/// Describes the type of domain object change for which the subscription should fire.
/// </summary>
[Flags]
public enum DomainObjectChangeType
{
    /// <summary>
    /// Domain object creation.
    /// </summary>
    Create = 1,

    /// <summary>
    /// Domain object change.
    /// </summary>
    Update = 2,

    /// <summary>
    /// Domain object deletion.
    /// </summary>
    Delete = 4,

    /// <summary>
    /// Any change.
    /// </summary>
    Any = Create | Update | Delete
}
