using RabbitMQ.Client;

namespace Framework.RabbitMq.Interfaces;

public interface IRabbitMqClient
{
    Task<IConnection?> TryConnectAsync(CancellationToken token, int? attempts = null);
}
