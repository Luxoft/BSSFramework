using Framework.DomainDriven.Repository;
using Framework.QueryableSource;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class RepositoryQueryableSource : IQueryableSource
{
    private readonly IServiceProvider serviceProvider;

    public RepositoryQueryableSource(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
    {
        return this.serviceProvider.GetRequiredKeyedService<IRepository<TDomainObject>>(SecurityRule.Disabled).GetQueryable();
    }
}
