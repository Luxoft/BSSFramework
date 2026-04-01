using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class DomainObjectEventBLL
{
    /// <inheritdoc />
    public QueueProcessingState GetProcessingState() =>
        new()
        {
            UnprocessedCount = this.GetUnsecureQueryable().Count<DomainObjectEvent>(mod => mod.Status != QueueProgressStatus.Processed),

            LastProcessedItemDateTime = this.GetUnsecureQueryable().Where<DomainObjectEvent>(mod => mod.Status == QueueProgressStatus.Processed).Max(mod => mod.ProcessDate)
        };
}
