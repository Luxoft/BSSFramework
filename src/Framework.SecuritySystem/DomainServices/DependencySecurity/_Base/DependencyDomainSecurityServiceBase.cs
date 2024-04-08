namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject> :

    DomainSecurityServiceBase<TDomainObject>
{
    private readonly IEnumerable<ISecurityRuleExpander> securityRuleExpanders;

    private readonly IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService;

    protected DependencyDomainSecurityServiceBase(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        IEnumerable<ISecurityRuleExpander> securityRuleExpanders,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
        : base(disabledSecurityProvider)
    {
        this.securityRuleExpanders = securityRuleExpanders;
        this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        var expandedSecurityRule = this.securityRuleExpanders.TryExpand<TDomainObject>(securityRule);

        if (expandedSecurityRule == null)
        {
            return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityRule));
        }
        else
        {
            return this.GetSecurityProvider(expandedSecurityRule);
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
