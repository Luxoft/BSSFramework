namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqSettings
{
    public RabbitMqServerSettings Server { get; set; } = default!;

    public string QueueName { get; set; } = default!;

    public int ReceiveMessageDelayMilliseconds { get; set; } = 500;
}
