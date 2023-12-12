using Framework.RabbitMq.Consumer.Enums;

namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqConsumerSettings
{
    public int ReceiveMessageDelayMilliseconds { get; set; } = 1000;

    public int RejectMessageDelayMilliseconds { get; set; } = 3000;

    public ulong FailedMessageRetryCount { get; set; } = 3;

    public int? ConnectionAttemptCount { get; set; }

    public string Exchange { get; set; } = default!;

    public string Queue { get; set; } = default!;

    public string[] RoutingKeys { get; set; } = Array.Empty<string>();

    public RabbitMqConsumerMode Mode { get; set; } = RabbitMqConsumerMode.Single;

    /// <summary>
    /// for multiple consumers mode - delay before next attempt to change consumer
    /// </summary>
    public int NoAvailableConsumersDelayMilliseconds { get; set; } = 60 * 1000;

    /// <summary>
    /// for multiple consumers mode - how often should consumer confirm availability
    /// </summary>
    public int ConsumerTickMilliseconds { get; set; } = 60 * 1000;

    /// <summary>
    /// for multiple consumers mode - how long can consumer be unavailable before it will be replaced
    /// </summary>
    public int ConsumerExpirationMilliseconds { get; set; } = 3 * 60 * 1000;
}
