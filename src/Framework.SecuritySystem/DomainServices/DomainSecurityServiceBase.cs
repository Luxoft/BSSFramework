using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityServiceBase<TDomainObject> : IDomainSecurityService<TDomainObject>
{
    private readonly IDictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>> modeProvidersCache;

    private readonly IDictionaryCache<SecurityOperation, ISecurityProvider<TDomainObject>> operationsProvidersCache;


    protected DomainSecurityServiceBase(IDisabledSecurityProviderSource disabledSecurityProviderSource)
    {
        this.operationsProvidersCache = new DictionaryCache<SecurityOperation, ISecurityProvider<TDomainObject>>(securityOperation =>
        {
            if (securityOperation is DisabledSecurityOperation)
            {
                return disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();
            }
            else
            {
                return this.CreateSecurityProvider(securityOperation);
            }
        }).WithLock();

        this.modeProvidersCache = new DictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>>(securityMode =>
        {
            if (securityMode == BLLSecurityMode.Disabled)
            {
                return disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();
            }
            else
            {
                return this.CreateSecurityProvider(securityMode);
            }
        }).WithLock();
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode);

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation);

    public ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.modeProvidersCache[securityMode];
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityMode)
    {
        return this.operationsProvidersCache[securityMode];
    }
}
