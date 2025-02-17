namespace Framework.DomainDriven.Lock;

public class ManualNamedLockSource(NamedLock namedLock) : INamedLockSource
{
    public IReadOnlyList<NamedLock> NamedLocks { get; } = [namedLock];
}
