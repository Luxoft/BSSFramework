using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.PersistStorage;

namespace Framework.DomainDriven;

public class DalStorageWriter(IServiceProvider serviceProvider) : IStorageWriter
{
    public async Task SaveAsync<TDomainObject>(TDomainObject data, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        var dal = serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>();

        await dal.SaveAsync(data, cancellationToken);
    }
}
