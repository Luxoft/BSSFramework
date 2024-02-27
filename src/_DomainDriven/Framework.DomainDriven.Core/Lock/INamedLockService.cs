namespace Framework.DomainDriven.Lock;

public interface INamedLockService
{
    Task LockAsync(NamedLock namedLock, LockRole lockRole, CancellationToken cancellationToken = default);
}
