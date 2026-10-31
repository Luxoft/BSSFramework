// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Queue processing state
/// </summary>
public class QueueProcessingState : DomainObjectBase
{
    /// <summary>
    /// Number of unprocessed records
    /// </summary>
    public int UnprocessedCount { get; set; }

    /// <summary>
    /// Processing time of the last record
    /// </summary>
    public DateTime? LastProcessedItemDateTime { get; set; }
}
