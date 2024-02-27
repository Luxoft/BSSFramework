namespace Framework.DomainDriven.Lock;

public interface INamedLockSource
{
    IReadOnlyList<NamedLock> NamedLocks { get; }
}
