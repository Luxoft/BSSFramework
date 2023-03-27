using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class DomainObjectNotificationBLL
{
    /// <inheritdoc />
    public QueueProcessingState GetProcessingState()
    {
        return new QueueProcessingState
               {
                       UnprocessedCount = this.GetUnsecureQueryable().Count(mod => mod.Status != QueueProgressStatus.Processed),

                       LastProcessedItemDateTime = this.GetUnsecureQueryable().Where(mod => mod.Status == QueueProgressStatus.Processed).Max(mod => mod.ProcessDate)
               };
    }
}
