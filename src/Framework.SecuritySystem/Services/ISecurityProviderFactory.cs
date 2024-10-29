namespace Framework.SecuritySystem.Services;

public interface ISecurityProviderFactory<TDomainObject, in TSecurityRule>
    where TSecurityRule : SecurityRule
{
    ISecurityProvider<TDomainObject> Create(TSecurityRule securityRule, SecurityPath<TDomainObject> securityPath);
}
