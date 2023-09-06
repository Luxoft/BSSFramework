using System.Linq.Expressions;

using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem.AccessDeniedExceptionService;

namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent,
                                                      TSecurityOperationCode> :

    DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent, TSecurityOperationCode>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TSecurityOperationCode : struct, Enum
    where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    protected DependencyDomainSecurityService(
        IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer,
        IDomainSecurityService<TBaseDomainObject, TSecurityOperationCode> baseDomainSecurityService,
        IQueryableSource<TPersistentDomainObjectBase> queryableSource)

        : base(disabledSecurityProviderContainer, baseDomainSecurityService)
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
