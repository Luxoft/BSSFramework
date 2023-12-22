using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqInitializer
{
    void Initialize(IModel model);
}
