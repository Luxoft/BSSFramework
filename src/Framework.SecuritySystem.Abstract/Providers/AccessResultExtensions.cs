namespace Framework.SecuritySystem;

public static class AccessResultExtensions
{
    public static AccessResult TryOverrideDomainObject<TDomainObject>(this AccessResult accessResult, TDomainObject domainObject) =>
        accessResult.TryOverrideAccessDeniedResult(
            accessDeniedResult => accessDeniedResult with { DomainObjectInfo = (domainObject, typeof(TDomainObject))! });

    public static AccessResult TryOverrideAccessDeniedResult(
        this AccessResult accessResult,
        Func<AccessResult.AccessDeniedResult, AccessResult.AccessDeniedResult> selector) =>
        accessResult switch
        {
            AccessResult.AccessDeniedResult accessDeniedResult => selector(accessDeniedResult),
            _ => accessResult
        };
}
