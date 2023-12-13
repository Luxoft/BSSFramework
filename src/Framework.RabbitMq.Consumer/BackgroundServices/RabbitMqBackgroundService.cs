using System.Text;

using Framework.RabbitMq.Consumer.Enums;
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

    private readonly IRabbitMqMessageProcessor _messageProcessor;

    private readonly IRabbitMqConsumerSemaphore _consumerSemaphore;

    private readonly RabbitMqServerSettings _serverSettings;

    private IModel? _channel;

    private IConnection? _connection;

    public RabbitMqBackgroundService(
        IRabbitMqClient client,
        IRabbitMqConsumerInitializer consumerInitializer,
        IRabbitMqMessageProcessor messageProcessor,
        IRabbitMqConsumerSemaphore consumerSemaphore,
        IOptions<RabbitMqServerSettings> serverOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        ILogger<RabbitMqBackgroundService> logger)
    {
        this._consumerId = Guid.NewGuid();
        this._client = client;
        this._consumerInitializer = consumerInitializer;
        this._messageProcessor = messageProcessor;
        this._consumerSemaphore = consumerSemaphore;
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

            await this.Listen(stoppingToken);
        }
    }

    private async Task Listen(CancellationToken token)
    {
        this._logger.LogInformation(
            "Listening RabbitMQ events has started on {Address}. Queue name is {Queue}",
            this._serverSettings.Address,
            this._consumerSettings.Queue);

        DateTime? obtainedSemaphoreAt = null;
        while (!token.IsCancellationRequested)
        {
            if (obtainedSemaphoreAt?.AddMilliseconds(this._consumerSettings.RefreshActiveConsumerTickMilliseconds) > DateTime.Now)
            {
                await this.ReadAsync(token);
            }
            else if (await this._consumerSemaphore.TryObtainAsync(this._consumerId, token) is (true, var newObtainedSemaphoreAt))
            {
                obtainedSemaphoreAt = newObtainedSemaphoreAt;
                this._logger.LogInformation("Current consumer is active");
                await this.ReadAsync(token);
            }
            else
            {
                await Delay(this._consumerSettings.RefreshActiveConsumerTickMilliseconds, token);
            }
        }

        await this._consumerSemaphore.TryReleaseAsync(this._consumerId, token);
    }

    private async Task ReadAsync(CancellationToken token)
    {
        var result = this._channel!.BasicGet(this._consumerSettings.Queue, false);
        if (result is null)
        {
            await Delay(this._consumerSettings.ReceiveMessageDelayMilliseconds, token);
            return;
        }

        await this.ProcessAsync(result, token);
    }

    public override void Dispose()
    {
        this._consumerSemaphore.TryReleaseAsync(this._consumerId, default).GetAwaiter().GetResult();
        this._channel?.Close();
        this._connection?.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task ProcessAsync(BasicGetResult result, CancellationToken token)
    {
        try
        {
            var message = Encoding.UTF8.GetString(result.Body.Span);
            if (result.Redelivered && result.DeliveryTag > this._consumerSettings.FailedMessageRetryCount)
            {
                var behaviour = await this._messageProcessor.ProcessDeadLetterAsync(
                                    result.BasicProperties,
                                    result.RoutingKey,
                                    message,
                                    token);
                if (behaviour == DeadLetterBehaviour.Skip)
                {
                    this._channel!.BasicAck(result.DeliveryTag, false);
                    return;
                }
            }

            await this._messageProcessor.ProcessAsync(result.BasicProperties, result.RoutingKey, message, token);
            this._channel!.BasicAck(result.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "There was some problem with processing message. Routing key:'{RoutingKey}'", result.RoutingKey);
            this._channel!.BasicNack(result.DeliveryTag, false, true);
            await Delay(this._consumerSettings.RejectMessageDelayMilliseconds, token);
        }
    }

    private static Task Delay(int value, CancellationToken token) => Task.Delay(TimeSpan.FromMilliseconds(value), token);
}
