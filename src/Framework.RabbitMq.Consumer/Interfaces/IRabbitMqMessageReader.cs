using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqMessageReader
{
    Task ReadAsync(IModel channel, CancellationToken token);
}
