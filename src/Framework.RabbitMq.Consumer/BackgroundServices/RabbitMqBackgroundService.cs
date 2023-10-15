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

    private readonly RabbitMqSettings _settings;

    private IModel _channel = default!;

    private IConnection? _connection;

    public RabbitMqBackgroundService(IRabbitMqClient client, IOptions<RabbitMqSettings> options, ILogger<RabbitMqBackgroundService> logger)
    {
        this._client = client;
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

    private async Task Listen(CancellationToken stoppingToken)
    {
        this._logger.LogInformation("Listening RabbitMQ events has started on {Host}:{Port}", this._settings.HostName, this._settings.Port);
        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(TimeSpan.FromMilliseconds(this._settings.ReceiveMessageDelayMilliseconds), stoppingToken);
    }

    public override void Dispose()
    {
        this._logger.LogInformation("Listening RabbitMQ events has stopped");

        this._channel.Close();
        this._connection?.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
