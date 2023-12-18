using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;
using Framework.RabbitMq.Interfaces;
using Framework.RabbitMq.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.BackgroundServices;

internal class RabbitMqBackgroundService(
    IRabbitMqClient client,
    IRabbitMqConsumerInitializer consumerInitializer,
    IRabbitMqConsumer consumer,
    IOptions<RabbitMqServerSettings> serverOptions,
    IOptions<RabbitMqConsumerSettings> consumerOptions,
    ILogger<RabbitMqBackgroundService> logger)
    : BackgroundService
{
    private readonly RabbitMqConsumerSettings _consumerSettings = consumerOptions.Value;

    private readonly RabbitMqServerSettings _serverSettings = serverOptions.Value;

    private IModel? _channel;

    private IConnection? _connection;

    private IRabbitMqClient Client { get; } = client;

    private IRabbitMqConsumerInitializer ConsumerInitializer { get; } = consumerInitializer;

    private IRabbitMqConsumer Consumer { get; } = consumer;

    private ILogger<RabbitMqBackgroundService> Logger { get; } = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._connection = await this.Client.TryConnectAsync(this._consumerSettings.ConnectionAttemptCount, stoppingToken);
        if (this._connection == null)
        {
            this.Logger.LogInformation("Listening RabbitMQ events wasn't started");
            return;
        }

        this._channel = this._connection!.CreateModel();
        this.ConsumerInitializer.Initialize(this._channel);

        this.Logger.LogInformation(
            "Listening RabbitMQ events has started on {Address}. Queue name is {Queue}, Consumer mode is {Mode}",
            this._serverSettings.Address,
            this._consumerSettings.Queue,
            this._consumerSettings.Mode);

        await this.Consumer.ConsumeAsync(this._channel!, stoppingToken);
    }

    public override void Dispose()
    {
        this.Consumer.Dispose();
        this._channel?.Close();
        this._connection?.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
