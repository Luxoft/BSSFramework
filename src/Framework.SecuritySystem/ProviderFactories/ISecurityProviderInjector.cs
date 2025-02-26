namespace Framework.SecuritySystem.ProviderFactories;

public interface ISecurityProviderInjector<TDomainObject, TSecurityRule> : ISecurityProviderInjector<TDomainObject>
    where TSecurityRule : SecurityRule
{
    Type ISecurityProviderInjector<TDomainObject>.SecurityRuleType => typeof(TSecurityRule);


    ISecurityProviderFactory<TDomainObject, TSecurityRule> Inject(ISecurityProviderFactory<TDomainObject, TSecurityRule> baseFactory) => baseFactory;
}

public interface ISecurityProviderInjector<TDomainObject>
{
    Type SecurityRuleType { get; }
}
