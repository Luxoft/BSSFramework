using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IDomainObjectEventBLL
{
    /// <summary>
    /// Getting the queue processing state
    /// </summary>
    /// <returns></returns>
    QueueProcessingState GetProcessingState();
}
