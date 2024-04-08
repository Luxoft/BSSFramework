namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject> : DomainSecurityServiceBase<TDomainObject>
{
    private readonly ISecurityRuleExpander securityRuleExpander;

    protected DomainSecurityService(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander)
        : base(disabledSecurityProvider)
    {
        this.securityRuleExpander = securityRuleExpander;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        var modeExpanded = this.securityRuleExpander.TryExpand<TDomainObject>(securityRule);

        if (modeExpanded != null)
        {

        }

        return this.GetSecurityProvider(this.securityOperationResolver.GetSecurityOperation<TDomainObject>(securityRule));
    }
}
