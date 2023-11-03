using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

public class OnlyDisabledDomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>
{
    private readonly ISecurityProvider<TDomainObject> disabledSecurityProvider;

    public OnlyDisabledDomainSecurityService(ISecurityProvider<TDomainObject> disabledSecurityProvider)
    {
        this.disabledSecurityProvider = disabledSecurityProvider;
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityOperation)
    {
        return this.GetSecurityProviderInternal(securityOperation, securityOperation is DisabledSecurityOperation);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.GetSecurityProviderInternal(securityMode, securityMode == BLLSecurityMode.Disabled);
    }

    private ISecurityProvider<TDomainObject> GetSecurityProviderInternal<TSecurityMode>(TSecurityMode securityMode, bool isDisabled)
    {
        if (isDisabled)
        {
            return this.disabledSecurityProvider;
        }
        else
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }
    }
}
