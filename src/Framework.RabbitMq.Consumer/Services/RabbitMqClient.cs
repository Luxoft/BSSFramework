using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;
using Polly.Retry;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqClient(IOptions<RabbitMqSettings> Options, ILogger<RabbitMqClient> Logger) : IRabbitMqClient
{
    private const int RetryConnectDelay = 5000;

    public async Task<IConnection?> TryConnectAsync(int? attempts = null)
    {
        var serverSettings = this.Options.Value.Server;
        var factory = new ConnectionFactory
                      {
                          HostName = serverSettings.Host,
                          Port = serverSettings.Port,
                          UserName = serverSettings.UserName,
                          Password = serverSettings.Secret,
                          VirtualHost = serverSettings.VirtualHost,
                          DispatchConsumersAsync = true,
                          AutomaticRecoveryEnabled = true
                      };

        var policy = this.CreateRetryPolicy(attempts);
        try
        {
            return await policy.ExecuteAsync(() => Task.FromResult(factory.CreateConnection()));
        }
        catch (Exception ex)
        {
            this.LogConnectionError(ex);
            return null;
        }
    }

    private AsyncRetryPolicy CreateRetryPolicy(int? attempts = null)
    {
        var builder = Policy.Handle<Exception>();
        if (attempts is null)
            return builder
                .WaitAndRetryForeverAsync(
                    _ => TimeSpan.FromMilliseconds(RetryConnectDelay),
                    (ex, _) => this.LogConnectionError(ex));

        return builder
            .WaitAndRetryAsync(
                attempts.Value,
                _ => TimeSpan.FromMilliseconds(RetryConnectDelay),
                (ex, _) => this.LogConnectionError(ex));
    }

    private void LogConnectionError(Exception exception) => this.Logger.LogError(exception, "Could not connect to RabbitMQ server");
}
