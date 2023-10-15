using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqAuditService
{
    Task ProcessAsync(IBasicProperties properties, string routingKey, string message, CancellationToken token);

    Task ProcessDeadLetterAsync(IBasicProperties properties, string routingKey, string message, CancellationToken token);
}
