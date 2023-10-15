using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqClient
{
    Task<IConnection?> TryConnectAsync();

    IModel CreateChannel(IConnection connection);
}
