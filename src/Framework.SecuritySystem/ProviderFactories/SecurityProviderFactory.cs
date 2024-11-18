namespace Framework.SecuritySystem.ProviderFactories;

public class SecurityProviderFactory<TDomainObject, TSecurityRule>(
    Func<TSecurityRule, SecurityPath<TDomainObject>, ISecurityProvider<TDomainObject>> createFunc)
    : ISecurityProviderFactory<TDomainObject, TSecurityRule>
    where TSecurityRule : SecurityRule
{
    public ISecurityProvider<TDomainObject> Create(TSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        return createFunc(securityRule, securityPath);
    }

    public static ISecurityProviderFactory<TDomainObject, TSecurityRule> Create(
        Func<TSecurityRule, SecurityPath<TDomainObject>, ISecurityProvider<TDomainObject>> createFunc) =>
        new SecurityProviderFactory<TDomainObject, TSecurityRule>(createFunc);
}
