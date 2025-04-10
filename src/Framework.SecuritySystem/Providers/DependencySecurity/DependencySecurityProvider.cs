using Framework.Core;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
    ISecurityProvider<TBaseDomainObject> baseSecurityProvider,
    IRelativeDomainPathInfo<TDomainObject, TBaseDomainObject> relativePath,
    IQueryableSource queryableSource)
    : ISecurityProvider<TDomainObject>
    where TBaseDomainObject : class
{
    public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
    {
        var baseDomainObjSecurityQ = queryableSource.GetQueryable<TBaseDomainObject>().Pipe(baseSecurityProvider.InjectFilter);

        return queryable.Where(relativePath.CreateCondition(domainObj => baseDomainObjSecurityQ.Contains(domainObj)));
    }

    public AccessResult GetAccessResult(TDomainObject domainObject)
    {
        return relativePath
               .GetRelativeObjects(domainObject)
               .Select(baseSecurityProvider.GetAccessResult)
               .Or()
               .TryOverrideDomainObject(domainObject);
    }

    public bool HasAccess(TDomainObject domainObject)
    {
        return relativePath.GetRelativeObjects(domainObject).Any(baseSecurityProvider.HasAccess);
    }

    public SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        return relativePath.GetRelativeObjects(domainObject).Select(baseSecurityProvider.GetAccessorData).Or();
    }
}
