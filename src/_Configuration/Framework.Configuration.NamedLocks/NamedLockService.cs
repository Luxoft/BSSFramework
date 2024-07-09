using Framework.Configuration.Domain;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Configuration.NamedLocks;

public class NamedLockService([DisabledSecurity] IRepository<GenericNamedLock> namedLockRepository) : INamedLockService
{
    public async Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken cancellationToken)
    {
        var genericNamedLock = await namedLockRepository.GetQueryable().Where(nl => nl.Name == namedLock.Name)
                                         .SingleAsync(cancellationToken);

        await namedLockRepository.LockAsync(genericNamedLock, lockRole, cancellationToken);
    }
}
