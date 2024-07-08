using Framework.DomainDriven.Repository;
using Framework.QueryableSource;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class RepositoryQueryableSource(IServiceProvider serviceProvider) : IQueryableSource
{
    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
    {
        return serviceProvider.GetRequiredKeyedService<IRepository<TDomainObject>>(nameof(SecurityRule.Disabled)).GetQueryable();
    }
}
