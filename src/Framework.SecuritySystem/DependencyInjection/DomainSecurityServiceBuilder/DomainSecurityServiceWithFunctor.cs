namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject> : DomainSecurityService<TDomainObject>
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    private readonly TOriginalDomainSecurityService originalDomainSecurityService;

    private readonly IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList;

    public DomainSecurityServiceWithFunctor(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        TOriginalDomainSecurityService originalDomainSecurityService,
        IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)

        : base(disabledSecurityProvider, securityRuleExpander)
    {
        this.originalDomainSecurityService = originalDomainSecurityService;
        this.functorList = functorList;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
    {
        var baseSecurityProvider = base.CreateSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            baseSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.OperationSecurityRule securityRule)
    {
        var baseSecurityProvider = base.CreateSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            baseSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        var baseSecurityProvider = base.CreateSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            baseSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        var originalSecurityProvider = this.originalDomainSecurityService.GetSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }
}
