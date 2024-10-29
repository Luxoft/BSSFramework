using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.ProviderFactories;

public class UntypedDependencySecurityProviderFactory<TDomainObject, TBaseDomainObject>(
    ISecurityModeExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
    IQueryableSource queryableSource)
    : DependencyBaseSecurityProviderFactory<TDomainObject,
        TBaseDomainObject>(securityRuleExpander, baseDomainSecurityService)
    where TDomainObject : IIdentityObject<Guid> where TBaseDomainObject : class, IIdentityObject<Guid>
{
    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new UntypedDependencySecurityProvider<TDomainObject, TBaseDomainObject>(
        baseProvider,
        queryableSource);
    }
}
