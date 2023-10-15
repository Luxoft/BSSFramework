namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqSettings
{
    public RabbitMqServerSettings Server { get; set; } = default!;

    public RabbitMqConsumerSettings Consumer { get; set; } = default!;
}
