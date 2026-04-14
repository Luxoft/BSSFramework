namespace Framework.Projection.Lambda.ProjectionSource;

/// <summary>
/// Источник проекций
/// </summary>
public interface IProjectionSource
{
    /// <summary>
    /// Получение списка проекций
    /// </summary>
    /// <returns></returns>
    IEnumerable<IProjection> GetProjections();
}
