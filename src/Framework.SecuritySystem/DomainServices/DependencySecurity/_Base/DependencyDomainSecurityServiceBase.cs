using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent> :

        IDomainSecurityService<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService;

    private readonly IDictionaryCache<SecurityOperation, ISecurityProvider<TDomainObject>> providersByOperationCache;

    private readonly IDictionaryCache<BLLSecurityMode, ISecurityProvider<TDomainObject>> modeProvidersCache;

    protected DependencyDomainSecurityServiceBase(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
    {
        this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));

        this.providersByOperationCache = new DictionaryCache<SecurityOperation, ISecurityProvider<TDomainObject>>(
         securityOperation => this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityOperation))).WithLock();

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


    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityOperation)
    {
        return this.providersByOperationCache[securityOperation];
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode);


    ISecurityProvider<TDomainObject> IDomainSecurityService<TDomainObject>.GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.modeProvidersCache[securityMode];
    }
}
