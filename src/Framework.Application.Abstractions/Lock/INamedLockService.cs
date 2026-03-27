using Framework.Application.Repository;
using Framework.Database;

namespace Framework.Application.Lock;

public interface INamedLockService
{
    Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken cancellationToken = default);
}
