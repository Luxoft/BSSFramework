using System.Linq.Expressions;

using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent> :

    DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    protected DependencyDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<TPersistentDomainObjectBase> securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
        IQueryableSource<TPersistentDomainObjectBase> queryableSource)

        : base(disabledSecurityProviderSource, securityOperationResolver, baseDomainSecurityService)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
    }

    protected abstract Expression<Func<TDomainObject, TBaseDomainObject>> Selector { get; }

    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent>(
            baseProvider,
            this.Selector,
            this.queryableSource);
    }
}
