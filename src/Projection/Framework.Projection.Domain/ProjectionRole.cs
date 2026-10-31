namespace Framework.Projection;

/// <summary>
/// Projection role
/// </summary>
public enum ProjectionRole
{
    /// <summary>
    /// Regular projection
    /// </summary>
    Default,

    /// <summary>
    /// Automatically completed projection (for transitions via ExpandPath)
    /// </summary>
    AutoNode,

    /// <summary>
    /// Projection for security
    /// </summary>
    SecurityNode
}
