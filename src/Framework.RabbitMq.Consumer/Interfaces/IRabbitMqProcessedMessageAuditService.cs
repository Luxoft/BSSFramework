using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqProcessedMessageAuditService
{
    Task SaveAsync(IBasicProperties properties, string routingKey, string message, CancellationToken token);
}
