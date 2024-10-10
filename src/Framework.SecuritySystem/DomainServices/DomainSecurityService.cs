using Framework.SecuritySystem.Expanders;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject>(ISecurityRuleExpander securityRuleExpander) : DomainSecurityServiceBase<TDomainObject>
{
    protected sealed override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.ModeSecurityRule modeSecurityRule:
                return this.CreateSecurityProvider(modeSecurityRule);

            case DomainModeSecurityRule domainModeSecurityRule:
                return this.CreateSecurityProvider(securityRuleExpander.Expand(domainModeSecurityRule));

            case OperationSecurityRule operationSecurityRule:
                return this.CreateSecurityProvider(operationSecurityRule);

            case NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.CreateSecurityProvider(nonExpandedRolesSecurityRule);

            case ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return this.CreateSecurityProvider(expandedRolesSecurityRule);

            case DomainSecurityRule domainObjectSecurityRule:
                return this.CreateFinalSecurityProvider(domainObjectSecurityRule);

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ModeSecurityRule securityRule)
    {
        return this.GetSecurityProvider(new DomainModeSecurityRule(typeof(TDomainObject), securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(OperationSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(NonExpandedRolesSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(ExpandedRolesSecurityRule securityRule)
    {
        return this.CreateFinalSecurityProvider(securityRule);
    }

    protected abstract ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(DomainSecurityRule securityRule);
}
