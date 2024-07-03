namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    TOriginalDomainSecurityService originalDomainSecurityService,
    IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)
    : DomainSecurityService<TDomainObject>(serviceProvider, disabledSecurityProvider, securityRuleExpander)
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        var expandedSpecialRules = securityRuleExpander.TryExpand<TDomainObject>(securityRule);

        var actualSecurityRules = expandedSpecialRules.Any() ? expandedSpecialRules : [securityRule];

        var originalSecurityProvider = actualSecurityRules.Select(originalDomainSecurityService.GetSecurityProvider).Or();

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.OperationSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.CompositeSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }
}
