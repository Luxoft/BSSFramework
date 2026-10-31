using Anch.Core;
using Anch.GenericQueryable;
using Anch.SecuritySystem.Attributes;

using Framework.Application.Repository;
using Framework.Database;

namespace Framework.Application.Lock;

public class NamedLockService<TGenericNamedLock>(
    [DisabledSecurity] IRepository<TGenericNamedLock> namedLockRepository,
    GenericNamedLockTypeInfo<TGenericNamedLock> genericNamedLockTypeInfo) : INamedLockService
{
    /// <summary>
    /// Takes a database-level lock on the row that represents <paramref name="namedLock"/> (see <see cref="NamedLockInitializer{TGenericNamedLock}"/>).
    /// Requires a write session (the DAL throws otherwise); the database lock lives until the current transaction ends, there is no explicit unlock.
    /// Throws if the named lock row doesn't exist: the lock must be declared in an <see cref="INamedLockSource"/> and initialized first.
    /// Security is disabled for the repository so locking works regardless of the current user's permissions.
    /// </summary>
    public async Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken ct)
    {
        var genericNamedLock = await namedLockRepository.GetQueryable()
                                                        .Where(genericNamedLockTypeInfo.NamePath.Select(nlName => nlName == namedLock.Name))
                                                        .GenericSingleAsync(ct);

        await namedLockRepository.LockAsync(genericNamedLock, lockRole, ct);
    }
}
