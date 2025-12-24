using CommonFramework.GenericRepository;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class DalGenericRepository(IServiceProvider serviceProvider) : IGenericRepository
{
    public async Task SaveAsync<TDomainObject>(TDomainObject data, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        var dal = serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>();

        await dal.SaveAsync(data, cancellationToken);
    }

    public async Task RemoveAsync<TDomainObject>(TDomainObject data, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        var dal = serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>();

        await dal.RemoveAsync(data, cancellationToken);
    }
}
