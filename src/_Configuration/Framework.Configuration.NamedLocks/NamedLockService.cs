using Framework.Configuration.Domain;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.Configuration.NamedLocks;

public class NamedLockService : INamedLockService
{
    private readonly IRepository<GenericNamedLock> namedLockRepository;

    public NamedLockService([FromKeyedServices(SecurityRule.Disabled)]IRepository<GenericNamedLock> namedLockRepository)
    {
        this.namedLockRepository = namedLockRepository;
    }

    public async Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken cancellationToken)
    {
        var genericNamedLock = await this.namedLockRepository.GetQueryable().Where(nl => nl.Name == namedLock.Name)
                                         .SingleAsync(cancellationToken);

        await this.namedLockRepository.LockAsync(genericNamedLock, lockRole, cancellationToken);
    }
}
