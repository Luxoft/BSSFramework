namespace Framework.SecuritySystem.ProviderFactories;

public interface IDefaultSecurityProviderFactory<TDomainObject, in TSecurityRule> : ISecurityProviderFactory<TDomainObject, TSecurityRule>
    where TSecurityRule : SecurityRule
{
    bool AllowOptimize => true;
}
