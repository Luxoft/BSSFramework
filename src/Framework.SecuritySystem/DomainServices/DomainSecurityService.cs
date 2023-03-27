using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>
        where TDomainObject : class
{
    private readonly IDictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>> providersCache;


    protected DomainSecurityService(IDisabledSecurityProviderContainer<TDomainObject> disabledSecurityProviderContainer)
    {
        this.providersCache = new DictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>>(securityMode =>
        {
            if (securityMode == BLLSecurityMode.Disabled)
            {
                return disabledSecurityProviderContainer.GetDisabledSecurityProvider<TDomainObject>();
            }
            else
            {
                return this.CreateSecurityProvider(securityMode);
            }
        }).WithLock();
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode);


    ISecurityProvider<TDomainObject> IDomainSecurityService<TDomainObject>.GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.providersCache[securityMode];
    }
}
