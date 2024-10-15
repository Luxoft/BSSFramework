using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    TOriginalDomainSecurityService originalDomainSecurityService,
    IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)
    : DomainSecurityService<TDomainObject>(securityRuleExpander)
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ModeSecurityRule securityRule)
    {
        var actualSecurityRule = (SecurityRule?)securityRuleExpander.TryExpand(securityRule.ToDomain<TDomainObject>()) ?? securityRule;

        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(actualSecurityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.OperationSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(DomainSecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        var originalSecurityProvider = originalDomainSecurityService.GetSecurityProvider(securityRule);

        return functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(DomainSecurityRule securityRule)
    {
        return originalDomainSecurityService.GetSecurityProvider(securityRule);
    }
}
