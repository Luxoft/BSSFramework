using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerInitializer
{
    void Initialize(IModel model);
}
