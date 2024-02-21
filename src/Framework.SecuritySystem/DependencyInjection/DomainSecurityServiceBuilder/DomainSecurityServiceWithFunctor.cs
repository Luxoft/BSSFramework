namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public class DomainSecurityServiceWithFunctor<TOriginalDomainSecurityService, TDomainObject> : DomainSecurityService<TDomainObject>
    where TOriginalDomainSecurityService : IDomainSecurityService<TDomainObject>
{
    private readonly TOriginalDomainSecurityService originalDomainSecurityService;

    private readonly IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList;

    public DomainSecurityServiceWithFunctor(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityOperationResolver securityOperationResolver,
        TOriginalDomainSecurityService originalDomainSecurityService,
        IEnumerable<IOverrideSecurityProviderFunctor<TDomainObject>> functorList)

        : base(disabledSecurityProvider, securityOperationResolver)
    {
        this.originalDomainSecurityService = originalDomainSecurityService;
        this.functorList = functorList;
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        var originalSecurityProvider = this.originalDomainSecurityService.GetSecurityProvider(securityOperation);

        return this.functorList.Aggregate(
            originalSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityOperation));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        var baseSecurityProvider = base.CreateSecurityProvider(securityMode);

        return this.functorList.Aggregate(
            baseSecurityProvider,
            (provider, functor) => functor.OverrideSecurityProvider(provider, securityMode));
    }
}
