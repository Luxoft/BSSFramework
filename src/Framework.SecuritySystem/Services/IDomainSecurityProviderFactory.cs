namespace Framework.SecuritySystem.Services;

public interface IDomainSecurityProviderFactory<TDomainObject>
{
    ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath);
}
