namespace Framework.SecuritySystem;

public interface IDomainSecurityProviderFactory
{
    ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.DomainSecurityRule securityRule);
}
