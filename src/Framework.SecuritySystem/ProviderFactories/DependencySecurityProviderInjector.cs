using Framework.QueryableSource;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.ProviderFactories;

public class DependencySecurityProviderFactory<TDomainObject, TBaseDomainObject>(
    ISecurityModeExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
    IQueryableSource queryableSource,
    IRelativeDomainPathInfo<TDomainObject, TBaseDomainObject> pathInfo)
    : DependencyBaseSecurityProviderFactory<TDomainObject,
        TBaseDomainObject>(securityRuleExpander, baseDomainSecurityService)
{
    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
            baseProvider,
            pathInfo.Path,
            queryableSource);
    }
}
