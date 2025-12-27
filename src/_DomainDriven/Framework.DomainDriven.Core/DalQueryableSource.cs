using CommonFramework.GenericRepository;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class DalQueryableSource(IServiceProvider serviceProvider) : IQueryableSource
{
    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class
    {
        return serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>().GetQueryable();
    }
}
