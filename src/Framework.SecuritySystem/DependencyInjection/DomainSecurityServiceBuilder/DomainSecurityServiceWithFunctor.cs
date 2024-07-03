namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    TOriginalDomainSecurityService originalDomainSecurityService,
    IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)
    : DomainSecurityService<TDomainObject>( disabledSecurityProvider, securityRuleExpander)
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        var actualSecurityRule = (SecurityRule)securityRuleExpander.TryExpand<TDomainObject>(securityRule) ?? securityRule;

        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(actualSecurityRule);

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

    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return originalDomainSecurityService.GetSecurityProvider(securityRule);
    }
}
