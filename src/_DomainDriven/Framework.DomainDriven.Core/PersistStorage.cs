using Framework.SecuritySystem.PersistStorage;

namespace Framework.DomainDriven;

public class PersistStorage<T>(IAsyncDal<T, Guid> dal) : IPersistStorage<T>
{
    public async Task SaveAsync(T data, CancellationToken cancellationToken) => await dal.SaveAsync(data, cancellationToken);
}
