using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class UntypedDependencyDomainSecurityService<TDomainObject, TBaseDomainObject, TIdent>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
    IQueryableSource queryableSource)
    : DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>(
        disabledSecurityProvider,
        securityRuleExpander,
        baseDomainSecurityService)
    where TDomainObject : IIdentityObject<TIdent>
    where TBaseDomainObject : class, IIdentityObject<TIdent>
{
    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new UntypedDependencySecurityProvider<TDomainObject, TBaseDomainObject, TIdent>(
            baseProvider,
            queryableSource);
    }
}
