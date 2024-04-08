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
        var customSecurityRule = this.securityRuleExpander.TryExpand<TDomainObject>(securityRule);

        if (customSecurityRule == null)
        {
            return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityRule));
        }
        else
        {
            return this.GetSecurityProvider(customSecurityRule);
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
