using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.PersistStorage;

namespace Framework.DomainDriven;

public class PersistStorage<T>([DisabledSecurity] IRepository<T> repository) : IPersistStorage<T>
{
    public async Task SaveAsync(T data, CancellationToken cancellationToken) => await repository.SaveAsync(data, cancellationToken);
}
