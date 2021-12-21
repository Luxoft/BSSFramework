using System.Collections.Generic;

namespace Framework.Projection.Lambda
{
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
}
