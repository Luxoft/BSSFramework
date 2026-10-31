namespace Framework.Projection.Lambda;

/// <summary>
/// Projection interface
/// </summary>
public interface IProjection : IProjectionAttributeProvider
{
    /// <summary>
    /// Type on which the projection is based
    /// </summary>
    Type SourceType { get; }

    /// <summary>
    /// Projection name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Generation of BLLViewRole logic and facades
    /// </summary>
    bool BLLView { get; }

    /// <summary>
    /// Projection role
    /// </summary>
    ProjectionRole Role { get; }

    /// <summary>
    /// List of properties
    /// </summary>
    IReadOnlyList<IProjectionProperty> Properties { get; }

    /// <summary>
    /// List of calculated properties
    /// </summary>
    IReadOnlyList<IProjectionCustomProperty> CustomProperties { get; }

    /// <summary>
    /// Additional filters
    /// </summary>
    IReadOnlyList<ProjectionFilterAttribute> FilterAttributes { get; }

    /// <summary>
    /// Ignore the Id property during serialization
    /// </summary>
    bool IgnoreIdSerialization { get; }
}
