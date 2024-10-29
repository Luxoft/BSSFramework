using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Services.DefaultInjectors;


public class RoleBaseSecurityProviderInjector<TDomainObject>(
    IRoleBaseSecurityProviderFactory<TDomainObject> roleBaseSecurityProviderFactory)
    : ISecurityProviderInjector<TDomainObject, RoleBaseSecurityRule>
{
    public ISecurityProvider<TDomainObject> Create(RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath) =>
        roleBaseSecurityProviderFactory.Create(securityRule, securityPath);
}
