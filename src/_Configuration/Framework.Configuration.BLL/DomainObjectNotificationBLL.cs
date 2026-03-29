using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class DomainObjectNotificationBLL
{
    /// <inheritdoc />
    public QueueProcessingState GetProcessingState()
    {
        return new QueueProcessingState
               {
                       UnprocessedCount = Queryable.Count<DomainObjectNotification>(this.GetUnsecureQueryable(), mod => mod.Status != QueueProgressStatus.Processed),

                       LastProcessedItemDateTime = Queryable.Where<DomainObjectNotification>(this.GetUnsecureQueryable(), mod => mod.Status == QueueProgressStatus.Processed).Max(mod => mod.ProcessDate)
               };
    }
}
