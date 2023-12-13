using Framework.DomainDriven;
using Framework.RabbitMq.Consumer.Interfaces;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqConsumerLockService<TDomainObject, TDomainObjectBase>(
    IRabbitMqConsumerLockProvider<TDomainObject> LockProvider,
    IAsyncDal<TDomainObjectBase, Guid> Dal) : IRabbitMqConsumerLockService
    where TDomainObject : TDomainObjectBase
{
    public async Task LockAsync(CancellationToken cancellationToken)
    {
        var domainObject = await this.LockProvider.GetLockObjectAsync(cancellationToken);
        await this.Dal.LockAsync(domainObject, LockRole.Update, cancellationToken);
    }
}
