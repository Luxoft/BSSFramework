using Framework.Core;

namespace Framework.SecuritySystem;

public class OverrideAccessDeniedResultSecurityProvider<TDomainObject>(
    ISecurityProvider<TDomainObject> baseProvider,
    Func<AccessResult.AccessDeniedResult, AccessResult.AccessDeniedResult> selector)
    : ISecurityProvider<TDomainObject>
{
    public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) => baseProvider.InjectFilter(queryable);

    public bool HasAccess(TDomainObject domainObject) => baseProvider.HasAccess(domainObject);

    public SecurityAccessorData GetAccessorData(TDomainObject domainObject) => baseProvider.GetAccessorData(domainObject);

    public AccessResult GetAccessResult(TDomainObject domainObject)
    {
        var accessResult = baseProvider.GetAccessResult(domainObject);

        if (accessResult is AccessResult.AccessDeniedResult accessDeniedResult)
        {
            return selector(accessDeniedResult);
        }
        else
        {
            return accessResult;
        }
    }
}
