using CommonFramework;

using Framework.DomainDriven.Repository;

using GenericQueryable;

using SecuritySystem.Attributes;

namespace Framework.DomainDriven.Lock;

public class NamedLockService<TGenericNamedLock>(
    [DisabledSecurity] IRepository<TGenericNamedLock> namedLockRepository,
    GenericNamedLockTypeInfo<TGenericNamedLock> genericNamedLockTypeInfo) : INamedLockService
{
    public async Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken cancellationToken)
    {
        var genericNamedLock = await namedLockRepository.GetQueryable()
                                                        .Where(genericNamedLockTypeInfo.NamePath.Select(nlName => nlName == namedLock.Name))
                                                        .GenericSingleAsync(cancellationToken);

        await namedLockRepository.LockAsync(genericNamedLock, lockRole, cancellationToken);
    }
}
