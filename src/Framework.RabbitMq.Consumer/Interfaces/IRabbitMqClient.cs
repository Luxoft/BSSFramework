using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqClient
{
    Task<IConnection?> TryConnectAsync(int? attempts = null);
}
