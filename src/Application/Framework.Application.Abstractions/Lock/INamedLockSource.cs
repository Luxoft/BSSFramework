namespace Framework.Application.Lock;

public interface INamedLockSource
{
    IReadOnlyList<NamedLock> NamedLocks { get; }
}
