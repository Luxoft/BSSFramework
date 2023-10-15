namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqConsumerSettings
{
    public int ReceiveMessageDelayMilliseconds { get; set; } = 500;

    public string Exchange { get; set; } = default!;

    public string Queue { get; set; } = default!;
}
