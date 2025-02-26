using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.ProviderFactories;

public abstract class
    DependencyBaseSecurityProviderFactory<TDomainObject, TBaseDomainObject>(
    ISecurityModeExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
    : IDefaultSecurityProviderFactory<TDomainObject, SecurityRule>
{
    public bool AllowOptimize { get; } = false;

    public ISecurityProvider<TDomainObject> Create(SecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        return this.CreateDependencySecurityProvider(baseDomainSecurityService.GetSecurityProvider(this.GetActualSecurityRule(securityRule)));
    }

    public SecurityRule GetActualSecurityRule(SecurityRule securityRule)
    {
        if (securityRule is SecurityRule.ModeSecurityRule modeSecurityRule
            && securityRuleExpander.TryExpand<TDomainObject>(modeSecurityRule) is { } customSecurityRule)
        {
            return customSecurityRule;
        }
        else
        {
            return securityRule;
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
