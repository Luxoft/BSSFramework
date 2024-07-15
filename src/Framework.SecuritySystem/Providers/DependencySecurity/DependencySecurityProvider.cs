using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem
{
    public class DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
        ISecurityProvider<TBaseDomainObject> baseSecurityProvider,
        Expression<Func<TDomainObject, TBaseDomainObject>> selector,
        IQueryableSource queryableSource)
        : ISecurityProvider<TDomainObject>
    {
        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            var baseDomainObjSecurityQ = queryableSource.GetQueryable<TBaseDomainObject>().Pipe(baseSecurityProvider.InjectFilter);

            return queryable.Where(selector.Select(domainObj => baseDomainObjSecurityQ.Contains(domainObj)));
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            return baseSecurityProvider.GetAccessResult(selector.Eval(domainObject)).TryOverrideDomainObject(domainObject);
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return baseSecurityProvider.HasAccess(selector.Eval(domainObject));
        }

        public SecurityAccessorData GetAccessorData(TDomainObject domainObject)
        {
            return baseSecurityProvider.GetAccessorData(selector.Eval(domainObject));
        }
    }
}
