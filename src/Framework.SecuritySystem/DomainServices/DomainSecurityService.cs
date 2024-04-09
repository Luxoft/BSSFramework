namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject> : DomainSecurityServiceBase<TDomainObject>
{
    private readonly IEnumerable<ISecurityRuleExpander> securityRuleExpanders;

    protected DomainSecurityService(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        IEnumerable<ISecurityRuleExpander> securityRuleExpanders)
        : base(disabledSecurityProvider)
    {
        this.securityRuleExpanders = securityRuleExpanders;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        var expandedSecurityRule = this.securityRuleExpanders.TryExpand<TDomainObject>(securityRule);

        if (expandedSecurityRule != null)
        {
            return this.GetSecurityProvider(expandedSecurityRule);
        }
        else
        {
            return this.CreateSecurityProvider(
                securityRule as SecurityRule.DomainObjectSecurityRule ?? throw new ArgumentOutOfRangeException(nameof(securityRule)));
        }
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule);
}
