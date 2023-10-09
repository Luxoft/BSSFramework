namespace Framework.SecuritySystem;

public static class AccessResultExtensions
{
    public static AccessResult TryOverrideDomainObject<TDomainObject>(this AccessResult accessResult, TDomainObject domainObject)
    {
        switch (accessResult)
        {
            case AccessResult.AccessDeniedResult accessDeniedResult:
                return accessDeniedResult with { DomainObjectInfo = (domainObject, typeof(TDomainObject)) };

            default:
                return accessResult;
        }
    }
}
