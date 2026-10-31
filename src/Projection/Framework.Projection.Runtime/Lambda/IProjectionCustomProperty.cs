namespace Framework.Projection.Lambda;

/// <summary>
/// Custom projection property
/// </summary>
public interface IProjectionCustomProperty : IProjectionAttributeProvider
{
    /// <summary>
    /// Property name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Writable
    /// </summary>
    bool Writable { get; }

    /// <summary>
    /// Property type
    /// </summary>
    TypeReferenceBase Type { get; }

    /// <summary>
    /// Additional property fetching
    /// </summary>
    IReadOnlyList<string> Fetchs { get; }
}
