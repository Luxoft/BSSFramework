using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqClient(IOptions<RabbitMqSettings> Options, ILogger<RabbitMqClient> Logger) : IRabbitMqClient
{
    private const int RetryConnectDelay = 5000;

    public async Task<IConnection?> TryConnectAsync()
    {
        var serverSettings = this.Options.Value.Server;
        var factory = new ConnectionFactory
                      {
                          HostName = serverSettings.Host,
                          Port = serverSettings.Port,
                          UserName = serverSettings.UserName,
                          Password = serverSettings.Secret,
                          VirtualHost = serverSettings.VirtualHost,
                          DispatchConsumersAsync = true,
                          AutomaticRecoveryEnabled = true
                      };

        var policy = Policy.Handle<Exception>()
                           .WaitAndRetryForeverAsync(
                               _ => TimeSpan.FromMilliseconds(RetryConnectDelay),
                               (ex, _) => this.Logger.LogError(ex, "Could not connect to RabbitMQ server"));

        return await policy.ExecuteAsync(() => Task.FromResult(factory.CreateConnection()));
    }

    public IModel CreateChannel(IConnection connection)
    {
        var consumerSettings = this.Options.Value.Consumer;

        var channel = connection.CreateModel();
        channel.ExchangeDeclare(consumerSettings.Exchange, ExchangeType.Topic, true);
        channel.QueueDeclare(consumerSettings.Queue, true, false, false, null);

        foreach (var routingKey in consumerSettings.RoutingKeys)
            channel.QueueBind(consumerSettings.Queue, consumerSettings.Exchange, routingKey);

        return channel;
    }
}
