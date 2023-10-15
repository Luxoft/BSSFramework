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

    private readonly ILogger<RabbitMqBackgroundService> _logger;

    private readonly IRabbitMqMessageProcessor _messageProcessor;

    private readonly RabbitMqSettings _settings;

    private IModel _channel = default!;

    private IConnection? _connection;

    public RabbitMqBackgroundService(
        IRabbitMqClient client,
        IRabbitMqMessageProcessor messageProcessor,
        IOptions<RabbitMqSettings> options,
        ILogger<RabbitMqBackgroundService> logger)
    {
        this._client = client;
        this._messageProcessor = messageProcessor;
        this._logger = logger;
        this._settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._connection = await this._client.TryConnectAsync();
        if (this._connection == null) return;

        this._channel = this._client.CreateChannel(this._connection);
        await this.Listen(stoppingToken);
    }

    private async Task Listen(CancellationToken token)
    {
        this._logger.LogInformation(
            "Listening RabbitMQ events has started on {Host}:{Port}",
            this._settings.Server.Host,
            this._settings.Server.Port);

        while (!token.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(this._settings.Consumer.ReceiveMessageDelayMilliseconds), token);

            var result = this._channel.BasicGet(this._settings.Consumer.Queue, false);
            if (result == null) continue;

            await this.ProcessAsync(result, token);
        }
    }

    public override void Dispose()
    {
        this._logger.LogInformation("Listening RabbitMQ events has stopped");

        this._channel.Close();
        this._connection?.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task ProcessAsync(BasicGetResult result, CancellationToken token)
    {
        try
        {
            var message = Encoding.UTF8.GetString(result.Body.Span);
            await this._messageProcessor.ProcessAsync(result.RoutingKey, message, token);

            this._channel.BasicAck(result.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Can not process RabbitMQ message with routing key '{RoutingKey}'", result.RoutingKey);
            this._channel.BasicNack(result.DeliveryTag, false, true);
        }
    }
}
