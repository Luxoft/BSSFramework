namespace Framework.SecuritySystem.PersistStorage;

public interface IPersistStorage<in T>
{
    Task SaveAsync(T data, CancellationToken cancellationToken);
}
