using Anch.GenericRepository;

using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application;

public class DalQueryableSource(IServiceProvider serviceProvider) : IQueryableSource
{
    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
        where TDomainObject : class =>
        serviceProvider.GetRequiredService<IAsyncDal<TDomainObject, Guid>>().GetQueryable();
}
