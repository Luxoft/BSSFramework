namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerLockProvider<TDomainObject>
{
    Task<TDomainObject> GetLockObjectAsync(CancellationToken cancellationToken);
}
