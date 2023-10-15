using RabbitMQ.Client;

namespace Framework.RabbitMq.Interfaces;

public interface IRabbitMqClient
{
    Task<IConnection?> TryConnectAsync(int? attempts = null);
}
