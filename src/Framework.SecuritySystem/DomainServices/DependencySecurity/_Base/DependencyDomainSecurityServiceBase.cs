namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
    : DomainSecurityServiceBase<TDomainObject>(disabledSecurityProvider)
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        if (securityRule is SecurityRule.SpecialSecurityRule specialSecurityRule
            && securityRuleExpander.TryExpand<TDomainObject>(specialSecurityRule) is { } customSecurityRules
            && customSecurityRules.Any())
        {
            return customSecurityRules.Select(this.CreateSecurityProvider).Or();
        }
        else
        {
            return this.CreateDependencySecurityProvider(baseDomainSecurityService.GetSecurityProvider(securityRule));
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
