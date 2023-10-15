namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqServerSettings
{
    public string Host { get; set; } = default!;

    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = default!;

    public string Secret { get; set; } = default!;

    public string VirtualHost { get; set; } = default!;
}
