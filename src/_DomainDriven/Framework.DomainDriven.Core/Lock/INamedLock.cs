namespace Framework.DomainDriven.Lock;

public interface INamedLock<out TNamedLockOperation>
        where TNamedLockOperation : struct, Enum
{
    TNamedLockOperation LockOperation { get; }
}
