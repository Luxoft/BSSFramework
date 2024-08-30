namespace Framework.SecuritySystem.Services;

public interface IRoleBaseSecurityProviderFactory<TDomainObject>
{
    ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath);
}
