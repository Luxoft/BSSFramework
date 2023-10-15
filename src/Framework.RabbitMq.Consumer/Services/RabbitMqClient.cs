using System.Net.Sockets;

using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqClient(IOptions<RabbitMqSettings> Options, ILogger<RabbitMqClient> Logger) : IRabbitMqClient
{
    private const int RetryConnectDelay = 5000;

    public async Task<IConnection?> TryConnectAsync()
    {
        var serverSettings = this.Options.Value.Server;
        var factory = new ConnectionFactory
                      {
                          HostName = serverSettings.HostName,
                          Port = serverSettings.Port,
                          UserName = serverSettings.UserName,
                          Password = serverSettings.Secret,
                          VirtualHost = serverSettings.VirtualHost,
                          DispatchConsumersAsync = true,
                          AutomaticRecoveryEnabled = true
                      };

        var policy = Policy.Handle<SocketException>()
                           .Or<BrokerUnreachableException>()
                           .WaitAndRetryForeverAsync(
                               _ => TimeSpan.FromMilliseconds(RetryConnectDelay),
                               (ex, _) => this.Logger.LogError(ex, "Could not connect to RabbitMQ"));

        return await policy.ExecuteAsync(() => Task.FromResult(factory.CreateConnection()));
    }

    public IModel CreateChannel(IConnection connection)
    {
        var channel = connection.CreateModel();
        channel.BasicQos(0, 1, false);
        channel.ExchangeDeclare(this.Options.Value.QueueName, ExchangeType.Topic, true);
        channel.QueueDeclare(this.Options.Value.QueueName, true, false, false, null);

        return channel;
    }
}
