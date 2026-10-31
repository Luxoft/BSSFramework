namespace Framework.Projection;

/// <summary>
/// Projection property role
/// </summary>
public enum ProjectionPropertyRole
{
    /// <summary>
    /// Regular property
    /// </summary>
    Default,

    /// <summary>
    /// Automatically completed property in the transition chain
    /// </summary>
    AutoNode,

    /// <summary>
    /// Last automatically completed property in the transition chain
    /// </summary>
    LastAutoNode,

    /// <summary>
    /// Automatically completed Parent property in an inner collection
    /// </summary>
    MissedParent,

    /// <summary>
    /// Automatically completed property for security
    /// </summary>
    Security,

    /// <summary>
    /// Calculated property
    /// </summary>
    Custom
}
