using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumer : IDisposable
{
    Task ConsumeAsync(IModel channel, CancellationToken token);
}
