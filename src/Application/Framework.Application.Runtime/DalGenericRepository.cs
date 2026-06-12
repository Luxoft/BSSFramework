using Anch.GenericRepository;

using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application;

public class DalGenericRepository(IServiceProvider serviceProvider) : IGenericRepository
{
    public async Task SaveAsync<TDomainObject>(TDomainObject data, CancellationToken ct)
        where TDomainObject : class
    {
        var dal = serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>();

        if (serviceProvider.GetService<IDalGenericInterceptor<TDomainObject>>() is { } interceptor)
        {
            await interceptor.SaveAsync(data, ct);
        }

        await dal.SaveAsync(data, ct);
    }

    public async Task RemoveAsync<TDomainObject>(TDomainObject data, CancellationToken ct)
        where TDomainObject : class
    {
        var dal = serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>();

        if (serviceProvider.GetService<IDalGenericInterceptor<TDomainObject>>() is { } interceptor)
        {
            await interceptor.RemoveAsync(data, ct);
        }

        await dal.RemoveAsync(data, ct);
    }
}

