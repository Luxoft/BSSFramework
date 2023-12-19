using Framework.RabbitMq.Consumer.Enums;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IDeadLetterProcessor
{
    Task<DeadLetterDecision> ProcessAsync(string message, string routingKey, Exception? exception, CancellationToken cancellationToken);
}
