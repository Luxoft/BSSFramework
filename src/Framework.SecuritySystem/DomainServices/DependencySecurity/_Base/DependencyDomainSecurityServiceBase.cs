namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject> :

    DomainSecurityServiceBase<TDomainObject>
{
    private readonly ISecurityRuleExpander securityRuleExpander;

    private readonly IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService;

    protected DependencyDomainSecurityServiceBase(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
        : base(disabledSecurityProvider)
    {
        this.securityRuleExpander = securityRuleExpander;
        this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        if (securityRule is SecurityRule.SpecialSecurityRule specialSecurityRule
            && this.securityRuleExpander.TryExpand<TDomainObject>(specialSecurityRule) is { } customSecurityRule)
        {
            return this.CreateSecurityProvider(customSecurityRule);
        }
        else
        {
            return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityRule));
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
