namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqMessageProcessor
{
    Task ProcessAsync(string routingKey, string message, CancellationToken token);
}
