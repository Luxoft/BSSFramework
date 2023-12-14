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

    public RabbitMqConsumerMode Mode { get; set; } = RabbitMqConsumerMode.MultipleActiveConsumers;

    /// <summary>
    /// for single active consumer mode - how often should consumer try to become active
    /// </summary>
    public int InactiveConsumerSleepMilliseconds { get; set; } = 60 * 1000;

    /// <summary>
    /// for single active consumer mode - how often should consumer update its active status
    /// </summary>
    public int ActiveConsumerRefreshMilliseconds { get; set; } = 3 * 60 * 1000;
}
