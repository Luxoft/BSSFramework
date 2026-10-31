namespace Framework.Projection.Lambda.ProjectionSource;

/// <summary>
/// Projection source
/// </summary>
public interface IProjectionSource
{
    /// <summary>
    /// Getting the list of projections
    /// </summary>
    /// <returns></returns>
    IEnumerable<IProjection> GetProjections();
}
