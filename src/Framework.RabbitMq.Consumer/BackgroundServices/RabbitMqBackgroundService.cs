using System.Text;

using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.BackgroundServices;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly IRabbitMqClient _client;

    private readonly IRabbitMqConsumerInitializer _consumerInitializer;

    private readonly IDeadLetterRabbitMqAuditService? _deadLetterAuditService;

    private readonly ILogger<RabbitMqBackgroundService> _logger;

    private readonly IRabbitMqMessageProcessor _messageProcessor;

    private readonly IProcessedMessageRabbitMqAuditService? _processedAuditService;

    private readonly RabbitMqSettings _settings;

    private IModel? _channel;

    private IConnection? _connection;

    public RabbitMqBackgroundService(
        IRabbitMqClient client,
        IRabbitMqConsumerInitializer consumerInitializer,
        IRabbitMqMessageProcessor messageProcessor,
        IProcessedMessageRabbitMqAuditService? processedAuditService,
        IDeadLetterRabbitMqAuditService? deadLetterAuditService,
        IOptions<RabbitMqSettings> options,
        ILogger<RabbitMqBackgroundService> logger)
    {
        this._client = client;
        this._consumerInitializer = consumerInitializer;
        this._messageProcessor = messageProcessor;
        this._processedAuditService = processedAuditService;
        this._deadLetterAuditService = deadLetterAuditService;
        this._logger = logger;
        this._settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._connection = await this._client.TryConnectAsync();
        this._channel = this._connection!.CreateModel();
        this._consumerInitializer.Initialize(this._channel);

        await this.Listen(stoppingToken);
    }

    private async Task Listen(CancellationToken token)
    {
        this._logger.LogInformation(
            "Listening RabbitMQ events has started on {Address}. Queue name is {Queue}",
            this._settings.Server.Address,
            this._settings.Consumer.Queue);

        while (!token.IsCancellationRequested)
        {
            await Delay(this._settings.Consumer.ReceiveMessageDelayMilliseconds, token);

            var result = this._channel!.BasicGet(this._settings.Consumer.Queue, false);
            if (result is null) continue;

            await this.ProcessAsync(result, token);
        }
    }

    public override void Dispose()
    {
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
            if (result.Redelivered
                && this._deadLetterAuditService is not null
                && result.DeliveryTag > this._settings.Consumer.FailedMessageRetryCount)
            {
                await this.LogAsync(this._deadLetterAuditService, result, message, token);
                this._channel!.BasicAck(result.DeliveryTag, false);
                return;
            }

            await this._messageProcessor.ProcessAsync(result.RoutingKey, message, token);
            await this.LogAsync(this._processedAuditService, result, message, token);
            this._channel!.BasicAck(result.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "There was some problem with processing message. Routing key:'{RoutingKey}'", result.RoutingKey);
            this._channel!.BasicNack(result.DeliveryTag, false, true);
            await Delay(this._settings.Consumer.RejectMessageDelayMilliseconds, token);
        }
    }

    private async Task LogAsync(IRabbitMqAuditService? service, BasicGetResult result, string message, CancellationToken token)
    {
        if (service is null) return;

        try
        {
            await service.ProcessAsync(result.BasicProperties, result.RoutingKey, message, token);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "There was some problem with logging message. Routing key:'{RoutingKey}'", result.RoutingKey);
        }
    }

    private static Task Delay(int value, CancellationToken token) => Task.Delay(TimeSpan.FromMilliseconds(value), token);
}
