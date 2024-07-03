using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class DependencyDomainSecurityService<TDomainObject, TBaseDomainObject>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
    IQueryableSource queryableSource,
    DependencyDomainSecurityServicePathInfo<TDomainObject, TBaseDomainObject> pathInfo)
    : DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>(
        disabledSecurityProvider,
        securityRuleExpander,
        baseDomainSecurityService)
{
    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
            baseProvider,
            pathInfo.Path,
            queryableSource);
    }
}
