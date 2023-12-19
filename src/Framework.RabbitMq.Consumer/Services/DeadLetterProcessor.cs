using Framework.RabbitMq.Consumer.Enums;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record DeadLetterProcessor(ILogger<DeadLetterProcessor> Logger, IOptions<RabbitMqConsumerSettings> ConsumerOptions)
    : IDeadLetterProcessor
{
    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    public Task<DeadLetterDecision> ProcessAsync(IModel channel, BasicGetResult message, Exception exception)
    {
        try
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>
                                 {
                                     { "routingKey", message.RoutingKey },
                                     { "queue", this._settings.Queue },
                                     { "error", exception.Message },
                                     { "stacktrace", exception.StackTrace ?? "missing stacktrace" }
                                 };

            channel.BasicPublish(this._settings.DeadLetterExchange, string.Empty, properties, message.Body.Span.ToArray());
            return Task.FromResult(DeadLetterDecision.RemoveFromQueue);
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Failed to process dead letter with routing key '{RoutingKey}'", message.RoutingKey);
            return Task.FromResult(DeadLetterDecision.Requeue);
        }
    }
}
