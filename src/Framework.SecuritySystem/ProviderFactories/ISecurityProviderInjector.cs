namespace Framework.SecuritySystem.ProviderFactories;

public interface ISecurityProviderInjector<TDomainObject, TSecurityRule>
    where TSecurityRule : SecurityRule
{
    ISecurityProviderFactory<TDomainObject, TSecurityRule> Inject(ISecurityProviderFactory<TDomainObject, TSecurityRule> baseFactory);
}
