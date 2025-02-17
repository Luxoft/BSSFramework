namespace Framework.DomainDriven.Lock;

public interface INamedLockInitializer
{
    Task Initialize(CancellationToken cancellationToken = default);
}
