using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.ProviderFactories;

public class RoleBaseSecurityProviderFactory<TDomainObject>(
    IRoleBaseSecurityProviderFactory<TDomainObject> roleBaseSecurityProviderFactory)
    : ISecurityProviderFactory<TDomainObject, DomainSecurityRule.RoleBaseSecurityRule>
{
    public ISecurityProvider<TDomainObject> Create(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath) =>
        roleBaseSecurityProviderFactory.Create(securityRule, securityPath);
}
