using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record SynchronizedConsumer(
    SqlConnectionStringProvider ConnectionStringProvider,
    IRabbitMqConsumerLockService LockService,
    ILogger<SynchronizedConsumer> Logger,
    IRabbitMqMessageReader MessageReader,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions)
    : IRabbitMqConsumer
{
    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    private SqlConnection? _connection;

    private DateTime? _lockObtainedDate;

    public async Task ConsumeAsync(IModel channel, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            try
            {
                if (await this.GetLock(token))
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

    public void Dispose()
    {
        if (this._connection != null) this.LockService.TryReleaseLock(this._connection);

        this._connection?.Close();
        GC.SuppressFinalize(this);
    }

    private async Task<bool> GetLock(CancellationToken token)
    {
        if (this._lockObtainedDate?.AddMilliseconds(this._settings.ActiveConsumerRefreshMilliseconds) >= DateTime.Now) return true;

        await this.OpenConnectionAsync(token);
        if (!this.LockService.TryObtainLock(this._connection!)) return false;

        this._lockObtainedDate = DateTime.Now;
        this.Logger.LogInformation("Current consumer is active");

        return true;
    }

    private async Task OpenConnectionAsync(CancellationToken token)
    {
        await this.CloseConnectionAsync();

        this._connection = new SqlConnection(this.ConnectionStringProvider.ConnectionString);
        await this._connection.OpenAsync(token);
    }

    private async Task CloseConnectionAsync()
    {
        try
        {
            if (this._connection != null)
            {
                this.LockService.TryReleaseLock(this._connection);
                await this._connection!.CloseAsync();
            }
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Failed to close connection");
        }
    }

    private static Task Delay(int value, CancellationToken token) => Task.Delay(TimeSpan.FromMilliseconds(value), token);
}
