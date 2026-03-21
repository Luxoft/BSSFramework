using CommonFramework.GenericRepository;

using Framework.Application.DAL;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application;

public class DalQueryableSource(IServiceProvider serviceProvider) : IQueryableSource
{
    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class
    {
        return serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>().GetQueryable();
    }
}
