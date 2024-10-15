using Framework.SecuritySystem.Expanders;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject>(ISecurityRuleExpander securityRuleExpander) : DomainSecurityServiceBase<TDomainObject>
{
    protected sealed override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule baseSecurityRule)
    {
        switch (baseSecurityRule)
        {
            case SecurityRule.ModeSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRule);

            case DomainModeSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRuleExpander.Expand(securityRule));

            case ClientSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRuleExpander.Expand(securityRule));

            case OperationSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRule);

            case NonExpandedRolesSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRule);

            case ExpandedRolesSecurityRule securityRule:
                return this.CreateSecurityProvider(securityRule);

            case DomainSecurityRule securityRule:
                return this.CreateFinalSecurityProvider(securityRule);

            default:
                throw new ArgumentOutOfRangeException(nameof(baseSecurityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ModeSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRule.ToDomain<TDomainObject>());
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
