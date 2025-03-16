using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class SecurityContextSource(IServiceProvider serviceProvider, IQueryableSource queryableSource) : ISecurityContextSource
{
    public IQueryable<TSecurityContext> GetQueryable<TSecurityContext>(SecurityContextRestrictionFilterInfo<TSecurityContext> filter)
        where TSecurityContext : class, ISecurityContext
    {
        return queryableSource.GetQueryable<TSecurityContext>().Where(filter.GetPureFilter(serviceProvider));
    }
}
