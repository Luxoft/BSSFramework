using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;
using Framework.RabbitMq.Interfaces;
using Framework.RabbitMq.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.BackgroundServices;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly Guid _consumerId;

    private readonly IRabbitMqClient _client;

    private readonly IRabbitMqConsumerInitializer _consumerInitializer;

    private readonly RabbitMqConsumerSettings _consumerSettings;

    private readonly ILogger<RabbitMqBackgroundService> _logger;

    private readonly IRabbitMqConsumer _consumer;

    private readonly RabbitMqServerSettings _serverSettings;

    private IModel? _channel;

    private IConnection? _connection;

    public RabbitMqBackgroundService(
        IRabbitMqClient client,
        IRabbitMqConsumerInitializer consumerInitializer,
        IRabbitMqConsumer consumer,
        IOptions<RabbitMqServerSettings> serverOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        ILogger<RabbitMqBackgroundService> logger)
    {
        this._consumerId = Guid.NewGuid();
        this._client = client;
        this._consumerInitializer = consumerInitializer;
        this._consumer = consumer;
        this._logger = logger;
        this._serverSettings = serverOptions.Value;
        this._consumerSettings = consumerOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (this._logger.BeginScope(new Dictionary<string, object> { ["ConsumerId"] = this._consumerId }))
        {
            this._connection = await this._client.TryConnectAsync(this._consumerSettings.ConnectionAttemptCount, stoppingToken);
            if (this._connection == null)
            {
                this._logger.LogInformation("Listening RabbitMQ events wasn't started");
                return;
            }

            this._channel = this._connection!.CreateModel();
            this._consumerInitializer.Initialize(this._channel);

            this._logger.LogInformation(
                "Listening RabbitMQ events has started on {Address}. Queue name is {Queue}",
                this._serverSettings.Address,
                this._consumerSettings.Queue);

            await this._consumer.ConsumeAsync(this._channel!, stoppingToken);
        }
    }

    public override void Dispose()
    {
        this._consumer.Dispose();
        this._channel?.Close();
        this._connection?.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
