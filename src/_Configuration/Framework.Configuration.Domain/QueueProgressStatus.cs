namespace Framework.Configuration.Domain;

/// <summary>
/// Object queue processing status
/// </summary>
public enum QueueProgressStatus
{
    /// <summary>
    /// Not processed
    /// </summary>
    Unprocessed = 0,

    /// <summary>
    /// In progress
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Processed
    /// </summary>
    Processed = 2
}
