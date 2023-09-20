using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityService<TPersistentDomainObjectBase, TDomainObject> : IDomainSecurityService<TDomainObject>
        where TDomainObject : TPersistentDomainObjectBase
{
    private readonly IDisabledSecurityProviderSource disabledSecurityProviderSource;

    private readonly ISecurityOperationResolver securityOperationResolver;

    private readonly IDictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>> modeProvidersCache;

    private readonly IDictionaryCache<SecurityOperation, ISecurityProvider<TDomainObject>> operationsProvidersCache;


    protected DomainSecurityService(IDisabledSecurityProviderSource disabledSecurityProviderSource,
                                    ISecurityOperationResolver securityOperationResolver)
    {
        this.disabledSecurityProviderSource = disabledSecurityProviderSource;
        this.securityOperationResolver = securityOperationResolver ?? throw new ArgumentNullException(nameof(securityOperationResolver));

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

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.CreateSecurityProvider(this.securityOperationResolver.GetSecurityOperation<TDomainObject>(securityMode));
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation);


    ISecurityProvider<TDomainObject> IDomainSecurityService<TDomainObject>.GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.modeProvidersCache[securityMode];
    }
    ISecurityProvider<TDomainObject> IDomainSecurityService<TDomainObject>.GetSecurityProvider(SecurityOperation securityMode)
    {
        return this.operationsProvidersCache[securityMode];
    }
}
