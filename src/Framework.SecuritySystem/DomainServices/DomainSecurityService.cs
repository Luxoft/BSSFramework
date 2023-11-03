namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject> : DomainSecurityServiceBase<TDomainObject>
{
    private readonly ISecurityOperationResolver securityOperationResolver;

    protected DomainSecurityService(
        ISecurityProvider<TDomainObject> disabledSecurityProvider,
        ISecurityOperationResolver securityOperationResolver)
        : base(disabledSecurityProvider)
    {
        this.securityOperationResolver = securityOperationResolver ?? throw new ArgumentNullException(nameof(securityOperationResolver));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.GetSecurityProvider(this.securityOperationResolver.GetSecurityOperation<TDomainObject>(securityMode));
    }
}
