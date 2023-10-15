namespace Framework.RabbitMq.Consumer.Settings;

public class RabbitMqSettings
{
    public string HostName { get; set; } = default!;

    public int Port { get; set; }

    public string UserName { get; set; } = default!;

    public string Secret { get; set; } = default!;

    public string VirtualHost { get; set; } = default!;

    public string QueueName { get; set; } = default!;

    public int ReceiveMessageDelayMilliseconds { get; set; } = 500;
}
