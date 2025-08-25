using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Services;

namespace Framework.DomainDriven;

public class AsyncDalQueryableSource(IServiceProvider serviceProvider) : IQueryableSource
{
    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class
    {
        return serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>().GetQueryable();
    }
}
