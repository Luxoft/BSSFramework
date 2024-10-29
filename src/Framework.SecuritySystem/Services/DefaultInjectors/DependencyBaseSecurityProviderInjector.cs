using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services.DefaultInjectors;

public abstract class
    DependencyBaseSecurityProviderInjector<TDomainObject, TBaseDomainObject>(
    ISecurityModeExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
    : ISecurityProviderInjector<TDomainObject, SecurityRule>
{
    public ISecurityProvider<TDomainObject> Create(SecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        return this.CreateDependencySecurityProvider(baseDomainSecurityService.GetSecurityProvider(this.GetActualSecurityRule(securityRule)));
    }

    public SecurityRule GetActualSecurityRule(SecurityRule securityRule)
    {
        if (securityRule is SecurityRule.ModeSecurityRule modeSecurityRule
            && securityRuleExpander.TryExpand(modeSecurityRule.ToDomain<TDomainObject>()) is { } customSecurityRule)
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
