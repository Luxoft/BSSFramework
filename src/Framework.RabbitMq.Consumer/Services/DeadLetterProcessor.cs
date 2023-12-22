using System.Text;

using Framework.RabbitMq.Consumer.Enums;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;
using Framework.RabbitMq.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record DeadLetterProcessor(
    IRabbitMqClient Client,
    ILogger<DeadLetterProcessor> Logger,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions)
    : IDeadLetterProcessor
{
    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    public async Task<DeadLetterDecision> ProcessAsync(
        string message,
        string routingKey,
        Exception? exception,
        CancellationToken cancellationToken)
    {
        try
        {
            using var connection = await this.Client.TryConnectAsync(this._settings.ConnectionAttemptCount, cancellationToken);
            if (connection == null) throw new Exception("Failed to open connection");

            using var channel = connection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>
                                 {
                                     { "routingKey", routingKey },
                                     { "queue", this._settings.Queue },
                                     { "error", exception?.GetBaseException().Message ?? "unknown exception" },
                                     { "stacktrace", exception?.StackTrace ?? "missing stacktrace" }
                                 };

            channel.BasicPublish(this._settings.DeadLetterExchange, string.Empty, properties, Encoding.UTF8.GetBytes(message));
            return DeadLetterDecision.RemoveFromQueue;
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Failed to process dead letter with routing key '{RoutingKey}'", routingKey);
            return DeadLetterDecision.Requeue;
        }
    }
}
