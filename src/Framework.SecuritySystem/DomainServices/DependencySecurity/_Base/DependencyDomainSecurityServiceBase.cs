using Framework.Persistent;

namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent> :

    DomainSecurityService<TPersistentDomainObjectBase, TDomainObject>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService;

    protected DependencyDomainSecurityServiceBase(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<TPersistentDomainObjectBase> securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService)
        : base(disabledSecurityProviderSource, securityOperationResolver)
    {
        this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityOperation));
    }

    protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);
}
