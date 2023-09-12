using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

public class OnlyDisabledDomainSecurityService<TDomainObject> : INotImplementedDomainSecurityService<TDomainObject>
{
    private readonly IDisabledSecurityProviderSource disabledSecurityProviderSource;

    public OnlyDisabledDomainSecurityService(IDisabledSecurityProviderSource disabledSecurityProviderSource)
    {
        this.disabledSecurityProviderSource = disabledSecurityProviderSource;
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
            return this.disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();
        }
        else
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }
    }
}
