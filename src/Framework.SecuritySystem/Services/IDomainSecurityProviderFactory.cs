namespace Framework.SecuritySystem.Services;

public interface IDomainSecurityProviderFactory
{
    ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule securityRule);
}
