using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class DependencyDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent> :

    DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    private readonly DependencyDomainSecurityServicePath<TDomainObject, TBaseDomainObject> path;

    public DependencyDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<TPersistentDomainObjectBase> securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
        IQueryableSource<TPersistentDomainObjectBase> queryableSource,
        DependencyDomainSecurityServicePath<TDomainObject, TBaseDomainObject> path)

        : base(disabledSecurityProviderSource, securityOperationResolver, baseDomainSecurityService)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
        this.path = path;
    }

    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent>(
            baseProvider,
            this.path.Selector,
            this.queryableSource);
    }
}
