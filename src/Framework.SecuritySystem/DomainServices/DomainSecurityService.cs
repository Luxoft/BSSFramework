namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject> : DomainSecurityServiceBase<TDomainObject>
{
    private readonly ISecurityOperationResolver securityOperationResolver;

    protected DomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver)
        : base(disabledSecurityProviderSource)
    {
        this.securityOperationResolver = securityOperationResolver ?? throw new ArgumentNullException(nameof(securityOperationResolver));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.CreateSecurityProvider(this.securityOperationResolver.GetSecurityOperation<TDomainObject>(securityMode));
    }
}
