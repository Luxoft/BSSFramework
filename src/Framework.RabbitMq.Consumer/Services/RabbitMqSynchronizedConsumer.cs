using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqSynchronizedConsumer(
    IRabbitMqSqlSeverConnectionStringProvider ConnectionStringProvider,
    IRabbitMqConsumerLockService LockService,
    ILogger<RabbitMqSynchronizedConsumer> Logger,
    IRabbitMqMessageReader MessageReader,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions)
    : IRabbitMqConsumer
{
    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    private SqlConnection? connection;

    public async Task ConsumeAsync(IModel channel, CancellationToken token)
    {
        DateTime? lockObtainedDate = null;
        while (!token.IsCancellationRequested)
        {
            try
            {
                var hasLock = false;

                if (lockObtainedDate?.AddMilliseconds(this._settings.ActiveConsumerRefreshMilliseconds) >= DateTime.Now)
                {
                    hasLock = true;
                }
                else
                {
                    await this.OpenConnectionAsync(token);
                    hasLock = this.LockService.TryObtainLock(this.connection!);
                    lockObtainedDate = hasLock ? DateTime.Now : null;
                    this.Logger.LogInformation("Current consumer is active");
                }

                if (hasLock)
                {
                    await this.MessageReader.ReadAsync(channel, token);
                }
                else
                {
                    await this.CloseConnectionAsync();
                    await Delay(this._settings.InactiveConsumerSleepMilliseconds, token);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Consuming error");
                await this.CloseConnectionAsync();
                await Delay(this._settings.InactiveConsumerSleepMilliseconds, token);
            }
        }
    }

    private async Task OpenConnectionAsync(CancellationToken token)
    {
        await this.CloseConnectionAsync();

        this.connection = new SqlConnection(this.ConnectionStringProvider.GetConnectionString());
        await this.connection.OpenAsync(token);
    }

    private async Task CloseConnectionAsync()
    {
        try
        {
            if (this.connection != null)
            {
                this.LockService.TryReleaseLock(this.connection);
                await this.connection!.CloseAsync();
            }
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Failed to close connection");
        }
    }

    public void Dispose()
    {
        if (this.connection != null)
        {
            this.LockService.TryReleaseLock(this.connection);
        }

        this.connection?.Close();
    }

    private static async Task Delay(int value, CancellationToken token) => await Task.Delay(TimeSpan.FromMilliseconds(value), token);
}
