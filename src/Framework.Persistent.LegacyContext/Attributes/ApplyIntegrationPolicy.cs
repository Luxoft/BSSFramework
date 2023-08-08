namespace Framework.Persistent;

/// <summary>
/// Константы, описывающие политику приема интеграционных сообщений
/// </summary>
public enum ApplyIntegrationPolicy
{
    /// <summary>
    /// The ignore integration message if message has less or equal version
    /// </summary>
    IgnoreLessOrEqualVersion = 0,

    /// <summary>
    /// The ignore integration message if message has less version
    /// </summary>
    IgnoreLessVersion = 1,
}
