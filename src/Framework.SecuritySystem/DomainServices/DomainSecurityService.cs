using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander)
    : DomainSecurityServiceBase<TDomainObject>(disabledSecurityProvider)
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

            case SecurityRule.CompositeSecurityRule compositeSecurityRule:
                return this.CreateSecurityProvider(compositeSecurityRule);

            case SecurityRule.CustomProviderSecurityRule customProviderSecurityRule:
                return (ISecurityProvider<TDomainObject>)serviceProvider.GetRequiredService(
                    customProviderSecurityRule.SecurityProviderType);

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        var actualSecurityRules = securityRuleExpander.TryExpand<TDomainObject>(securityRule);

        if (!actualSecurityRules.Any())
        {
            throw new Exception($"{nameof(SecurityRule)} with mode '{securityRule}' not found for type '{typeof(TDomainObject).Name}'");
        }

        return actualSecurityRules.Select(this.GetSecurityProvider).Or();
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.GetSecurityProvider(securityRuleExpander.Expand(securityRule));
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.CompositeSecurityRule securityRule);

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule);
}
