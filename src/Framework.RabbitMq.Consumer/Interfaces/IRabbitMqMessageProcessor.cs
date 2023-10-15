using Framework.RabbitMq.Consumer.Enums;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqMessageProcessor
{
    Task ProcessAsync(IBasicProperties properties, string routingKey, string message, CancellationToken token);

    Task<DeadLetterBehaviour> ProcessDeadLetterAsync(
        IBasicProperties properties,
        string routingKey,
        string message,
        CancellationToken token) =>
        Task.FromResult(DeadLetterBehaviour.ForeverRetry);
}
