namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject>(ISecurityRuleExpander securityRuleExpander) : DomainSecurityServiceBase<TDomainObject>
{
    protected sealed override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.SpecialSecurityRule specialSecurityRule:
                return this.CreateSecurityProvider(specialSecurityRule);

            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return this.CreateSecurityProvider(operationSecurityRule);

            case SecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.CreateSecurityProvider(nonExpandedRolesSecurityRule);

            case SecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return this.CreateSecurityProvider(expandedRolesSecurityRule);

            case SecurityRule.DomainObjectSecurityRule domainObjectSecurityRule:
                return this.CreateFinalSecurityProvider(domainObjectSecurityRule);

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        return this.GetSecurityProvider(
            securityRuleExpander.TryExpand<TDomainObject>(securityRule)
            ?? throw new ArgumentOutOfRangeException(nameof(securityRule), $"{nameof(SecurityRule)} with mode '{securityRule}' not found for type '{typeof(TDomainObject).Name}'"));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        return this.CreateFinalSecurityProvider(securityRule);
    }

    protected abstract ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule);
}
