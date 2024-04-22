namespace Framework.SecuritySystem;

public interface ISecurityPathProviderFactory
{
    ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.DomainObjectSecurityRule securityRule);
}
