namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqConsumerSettings
{
    public int ReceiveMessageDelayMilliseconds { get; set; } = 1000;

    public int RejectMessageDelayMilliseconds { get; set; } = 3000;

    public ulong FailedMessageRetryCount { get; set; } = 3;

    public string Exchange { get; set; } = default!;

    public string Queue { get; set; } = default!;

    public string[] RoutingKeys { get; set; } = Array.Empty<string>();
}
