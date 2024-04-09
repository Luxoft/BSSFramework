namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject> : DomainSecurityService<TDomainObject>
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    private readonly TOriginalDomainSecurityService originalDomainSecurityService;

    private readonly IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList;

    public DomainSecurityServiceWithFunctor(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        IEnumerable<ISecurityRuleExpander> securityRuleExpanders,
        TOriginalDomainSecurityService originalDomainSecurityService,
        IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)

        : base(disabledSecurityProvider, securityRuleExpanders)
    {
        this.originalDomainSecurityService = originalDomainSecurityService;
        this.functorList = functorList;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule)
    {
        var baseSecurityProvider = base.CreateSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            baseSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        var originalSecurityProvider = this.originalDomainSecurityService.GetSecurityProvider(securityRule);

        return this.functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityRule));
    }
}
