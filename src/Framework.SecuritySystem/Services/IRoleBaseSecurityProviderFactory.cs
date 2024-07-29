namespace Framework.SecuritySystem.Services;

public interface IRoleBaseSecurityProviderFactory
{
    ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
