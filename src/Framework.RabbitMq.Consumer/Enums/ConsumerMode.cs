namespace Framework.RabbitMq.Consumer.Enums;

public enum ConsumerMode
{
    /// <summary>
    ///     Allows concurrent consuming
    /// </summary>
    MultipleActiveConsumers = 0,

    /// <summary>
    ///     Restricts to consequential consuming
    /// </summary>
    SingleActiveConsumer = 1
}
