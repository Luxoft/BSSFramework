using System.Text;

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
    private readonly IRabbitMqAuditService? _auditService;

    private readonly IRabbitMqClient _client;

    private readonly IRabbitMqConsumerInitializer _consumerInitializer;

    private readonly RabbitMqConsumerSettings _consumerSettings;

    private readonly ILogger<RabbitMqBackgroundService> _logger;

    private readonly IRabbitMqMessageProcessor _messageProcessor;

    private readonly RabbitMqServerSettings _serverSettings;

    private IModel? _channel;

    private IConnection? _connection;

    public RabbitMqBackgroundService(
        IRabbitMqClient client,
        IRabbitMqConsumerInitializer consumerInitializer,
        IRabbitMqMessageProcessor messageProcessor,
        IRabbitMqAuditService? auditService,
        IOptions<RabbitMqServerSettings> serverOptions,
        IOptions<RabbitMqConsumerSettings> consumerOptions,
        ILogger<RabbitMqBackgroundService> logger)
    {
        this._client = client;
        this._consumerInitializer = consumerInitializer;
        this._messageProcessor = messageProcessor;
        this._auditService = auditService;
        this._logger = logger;
        this._serverSettings = serverOptions.Value;
        this._consumerSettings = consumerOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._connection = await this._client.TryConnectAsync(stoppingToken);
        this._channel = this._connection!.CreateModel();
        this._consumerInitializer.Initialize(this._channel);

        await this.Listen(stoppingToken);
    }

    private async Task Listen(CancellationToken token)
    {
        this._logger.LogInformation(
            "Listening RabbitMQ events has started on {Address}. Queue name is {Queue}",
            this._serverSettings.Address,
            this._consumerSettings.Queue);

        while (!token.IsCancellationRequested)
        {
            var result = this._channel!.BasicGet(this._consumerSettings.Queue, false);
            if (result is null)
            {
                await Delay(this._consumerSettings.ReceiveMessageDelayMilliseconds, token);
                continue;
            }

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
                && this._auditService is not null
                && result.DeliveryTag > this._consumerSettings.FailedMessageRetryCount)
            {
                await this.LogDeadLetterAsync(result, message, token);
                this._channel!.BasicAck(result.DeliveryTag, false);
                return;
            }

            await this._messageProcessor.ProcessAsync(result.RoutingKey, message, token);
            await this.LogProcessedAsync(result, message, token);
            this._channel!.BasicAck(result.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "There was some problem with processing message. Routing key:'{RoutingKey}'", result.RoutingKey);
            this._channel!.BasicNack(result.DeliveryTag, false, true);
            await Delay(this._consumerSettings.RejectMessageDelayMilliseconds, token);
        }
    }

    private async Task LogDeadLetterAsync(BasicGetResult result, string message, CancellationToken token)
    {
        try
        {
            await this._auditService!.ProcessDeadLetterAsync(result.BasicProperties, result.RoutingKey, message, token);
        }
        catch
        {
            // ignored
        }
    }

    private async Task LogProcessedAsync(BasicGetResult result, string message, CancellationToken token)
    {
        if (this._auditService is null) return;

        try
        {
            await this._auditService!.ProcessAsync(result.BasicProperties, result.RoutingKey, message, token);
        }
        catch
        {
            // ignored
        }
    }

    private static Task Delay(int value, CancellationToken token) => Task.Delay(TimeSpan.FromMilliseconds(value), token);
}
