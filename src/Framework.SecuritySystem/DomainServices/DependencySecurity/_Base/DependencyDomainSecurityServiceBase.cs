namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject> :

    DomainSecurityServiceBase<TDomainObject>
{
    private readonly ISecurityOperationResolver securityOperationResolver;

    private readonly IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService;

    protected DependencyDomainSecurityServiceBase(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
        : base(disabledSecurityProviderSource)
    {
        this.securityOperationResolver = securityOperationResolver;
        this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        var customSecurityOperation = this.securityOperationResolver.TryGetSecurityOperation<TDomainObject>(securityMode);

        if (customSecurityOperation == null)
        {
            return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityMode));
        }
        else
        {
            return this.GetSecurityProvider(customSecurityOperation);
        }
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityOperation));
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
