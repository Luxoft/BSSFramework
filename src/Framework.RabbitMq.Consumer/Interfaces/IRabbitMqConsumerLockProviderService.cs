namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerLockProviderService<TDomainObject>
{
    Task<TDomainObject> GetLockObjectAsync(CancellationToken cancellationToken);
}
