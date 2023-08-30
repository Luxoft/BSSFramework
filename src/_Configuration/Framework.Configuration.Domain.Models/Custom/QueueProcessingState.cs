namespace Framework.Configuration.Domain;

/// <summary>
/// Состояние обработки очереди
/// </summary>
public class QueueProcessingState : DomainObjectBase
{
    /// <summary>
    /// Количество необработанных записей
    /// </summary>
    public int UnprocessedCount { get; set; }

    /// <summary>
    /// Время обработки последней записи
    /// </summary>
    public DateTime? LastProcessedItemDateTime { get; set; }
}
