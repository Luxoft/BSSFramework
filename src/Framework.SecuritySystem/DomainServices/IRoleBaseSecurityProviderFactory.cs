namespace Framework.SecuritySystem;

public interface IRoleBaseSecurityProviderFactory
{
    ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.RoleBaseSecurityRule securityRule);
}
