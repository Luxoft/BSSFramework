namespace Framework.RabbitMq.Settings;

public class RabbitMqServerSettings
{
    public string Host { get; set; } = default!;

    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = default!;

    public string Secret { get; set; } = default!;

    public string VirtualHost { get; set; } = default!;

    public Uri Address => new($"{this.Host}:{this.Port}", UriKind.Absolute);

    public Uri ConnectionUri =>
        new($"amqp://{this.UserName}:{this.Secret}@{this.Host}:{this.Port}/{Uri.EscapeDataString(this.VirtualHost)}");
}
