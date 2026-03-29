using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class DomainObjectEventBLL
{
    /// <inheritdoc />
    public QueueProcessingState GetProcessingState()
    {
        return new QueueProcessingState
               {
                       UnprocessedCount = Queryable.Count<DomainObjectEvent>(this.GetUnsecureQueryable(), mod => mod.Status != QueueProgressStatus.Processed),

                       LastProcessedItemDateTime = Queryable.Where<DomainObjectEvent>(this.GetUnsecureQueryable(), mod => mod.Status == QueueProgressStatus.Processed).Max(mod => mod.ProcessDate)
               };
    }
}
