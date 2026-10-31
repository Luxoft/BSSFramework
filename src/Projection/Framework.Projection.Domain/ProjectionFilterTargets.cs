namespace Framework.Projection;

/// <summary>
/// Projection filter applicability
/// </summary>
[Flags]
public enum ProjectionFilterTargets
{
    /// <summary>
    /// For fetching regular collections
    /// </summary>
    Collection = 1,

    /// <summary>
    /// For OData fetching
    /// </summary>
    OData = 2,

    /// <summary>
    /// For fetching a single instance
    /// </summary>
    Single = 4,

    /// <summary>
    /// For tree fetching
    /// </summary>
    ODataTree = 8,

    /// <summary>
    /// All variants of filter applicability
    /// </summary>
    All = Collection + OData + Single + ODataTree
}
