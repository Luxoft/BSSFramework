namespace Framework.Configuration.Domain;

/// <summary>
/// Константы, описывающие перечисление результатов состояний работы с контекстами
/// </summary>
public enum SubscriptionSourceMode
{
    /// <summary>
    /// Если заданы одновременно типизированный и нетипизированный контексты, то вернется состояние "Invalid"
    /// </summary>
    Invalid,

    /// <summary>
    /// Если не задан контекст, то ничего не будет возвращено
    /// </summary>
    NonContext,

    /// <summary>
    /// Если задан нетипизированный контекст, вернется значение DynamicSource
    /// </summary>
    Dynamic,

    /// <summary>
    /// Если задан типизированный контекст, вернется значение AuthSource
    /// </summary>
    Typed
}
