namespace Framework.Configuration.NamedLocks;

public interface INamedLockInitializer
{
    Task Initialize(CancellationToken cancellationToken = default);
}
