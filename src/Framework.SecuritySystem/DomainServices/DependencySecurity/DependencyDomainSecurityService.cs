using System;
using System.Linq.Expressions;

using Framework.Persistent;
using Framework.QueryableSource;

using JetBrains.Annotations;

namespace Framework.SecuritySystem;

public abstract class DependencyDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent, TSecurityOperationCode> :

        DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
        where TBaseDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService;

    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    protected DependencyDomainSecurityService(
            [NotNull] IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] IDomainSecurityService<TBaseDomainObject, TSecurityOperationCode> baseDomainSecurityService,
            [NotNull] IQueryableSource<TPersistentDomainObjectBase> queryableSource)

            : base(disabledSecurityProviderContainer, baseDomainSecurityService)
    {
        this.accessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
    }

    protected abstract Expression<Func<TDomainObject, TBaseDomainObject>> Selector { get; }

    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent>(this.accessDeniedExceptionService, baseProvider, this.Selector, this.queryableSource);
    }
}
