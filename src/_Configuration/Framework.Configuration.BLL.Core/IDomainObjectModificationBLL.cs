using Framework.Configuration.Domain;
using Framework.Core;

namespace Framework.Configuration.BLL;

public partial interface IDomainObjectModificationBLL
{
    /// <summary>
    /// Processing of modifications stored in the database
    /// </summary>
    /// <param name="limit">Limit on the number of processed modifications</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of processed modifications</returns>
    Task<ITryResult<int>> ProcessAsync(int limit = 1000, CancellationToken ct = default);

    /// <summary>
    /// Getting the queue processing state
    /// </summary>
    /// <returns></returns>
    QueueProcessingState GetProcessingState();
}
