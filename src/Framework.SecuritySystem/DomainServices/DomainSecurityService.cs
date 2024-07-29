using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject>(ISecurityRuleExpander securityRuleExpander) : DomainSecurityServiceBase<TDomainObject>
{
    protected sealed override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.ModeSecurityRule specialSecurityRule:
                return this.CreateSecurityProvider(specialSecurityRule);

            case DomainSecurityRule.OperationSecurityRule operationSecurityRule:
                return this.CreateSecurityProvider(operationSecurityRule);

            case DomainSecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.CreateSecurityProvider(nonExpandedRolesSecurityRule);

            case DomainSecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return this.CreateSecurityProvider(expandedRolesSecurityRule);

            case DomainSecurityRule domainObjectSecurityRule:
                return this.CreateFinalSecurityProvider(domainObjectSecurityRule);

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ModeSecurityRule securityRule)
    {
        return this.GetSecurityProvider(
            securityRuleExpander.TryExpand<TDomainObject>(securityRule)
            ?? throw new ArgumentOutOfRangeException(nameof(securityRule), $"{nameof(SecurityRule)} with mode '{securityRule}' not found for type '{typeof(TDomainObject).Name}'"));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.OperationSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        return this.CreateFinalSecurityProvider(securityRule);
    }

    protected abstract ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(DomainSecurityRule securityRule);
}
