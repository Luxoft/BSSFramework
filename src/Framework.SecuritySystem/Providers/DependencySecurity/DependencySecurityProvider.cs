using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class DependencySecurityProvider<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent> : SecurityProviderBase<TDomainObject>

        where TDomainObject : class, TPersistentDomainObjectBase
        where TBaseDomainObject : class, TPersistentDomainObjectBase
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    private readonly Expression<Func<TDomainObject, TBaseDomainObject>> selector;

    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    private readonly ISecurityProvider<TBaseDomainObject> baseSecurityProvider;

    public DependencySecurityProvider(IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService, ISecurityProvider<TBaseDomainObject> baseSecurityProvider, Expression<Func<TDomainObject, TBaseDomainObject>> selector, IQueryableSource<TPersistentDomainObjectBase> queryableSource)
            : base(accessDeniedExceptionService)
    {
        this.baseSecurityProvider = baseSecurityProvider ?? throw new ArgumentNullException(nameof(baseSecurityProvider));
        this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        this.queryableSource = queryableSource;
    }

    public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
    {
        var baseDomainObjSecurityQ = this.queryableSource.GetQueryable<TBaseDomainObject>().Pipe(this.baseSecurityProvider.InjectFilter);

        return queryable.Where(this.selector.Select(domainObj => baseDomainObjSecurityQ.Contains(domainObj)));
    }

    public override bool HasAccess(TDomainObject domainObject)
    {
        return this.baseSecurityProvider.HasAccess(this.selector.Eval(domainObject));
    }

    public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
    {
        return this.baseSecurityProvider.GetAccessors(this.selector.Eval(domainObject));
    }
}
