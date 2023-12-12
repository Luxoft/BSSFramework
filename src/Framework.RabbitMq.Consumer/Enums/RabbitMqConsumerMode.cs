namespace Framework.RabbitMq.Consumer.Enums;

public enum RabbitMqConsumerMode
{
    /// <summary>
    /// Singe app/pod/etc
    /// </summary>
    Single = 0,

    /// <summary>
    /// Multiple apps/pods/etc
    /// </summary>
    Multiple = 1
}
