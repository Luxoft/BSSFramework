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

    protected sealed override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.SpecialSecurityRule specialSecurityRule:
                return this.CreateSecurityProvider(specialSecurityRule);

            case SpecialRoleSecurityRule specialRoleSecurityRule:
                return this.CreateSecurityProvider(specialRoleSecurityRule);

            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return this.CreateSecurityProvider(operationSecurityRule);

            case SecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.CreateSecurityProvider(nonExpandedRolesSecurityRule);

            case SecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return this.CreateSecurityProvider(expandedRolesSecurityRule);

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        return this.GetSecurityProvider(
                   this.securityRuleExpander.TryExpand<TDomainObject>(securityRule))
               ?? throw new Exception($"SecurityRule with mode '{securityRule}' not found for type '{typeof(TDomainObject).Name}'");
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SpecialRoleSecurityRule securityRule)
    {
        return this.GetSecurityProvider(this.securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.GetSecurityProvider(this.securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.GetSecurityProvider(this.securityRuleExpander.Expand(securityRule));
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule);
}
