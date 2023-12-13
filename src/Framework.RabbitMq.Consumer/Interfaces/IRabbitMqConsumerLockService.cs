namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerLockService
{
    Task LockAsync(CancellationToken cancellationToken);
}
