namespace Framework.Configuration.Domain;

/// <summary>
/// Статус обработки очереди объектов
/// </summary>
public enum QueueProgressStatus
{
    /// <summary>
    /// Необработано
    /// </summary>
    Unprocessed = 0,

    /// <summary>
    /// В процессе обработки
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Обработано
    /// </summary>
    Processed = 2
}
