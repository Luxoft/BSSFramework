using Framework.Core;

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

    public static AccessResult Or(this IEnumerable<AccessResult> source) =>
        source.Match(
            () => AccessResult.AccessDeniedResult.Default,
            result => result,
            results => results.Aggregate((AccessResult)AccessResult.AccessDeniedResult.Default, (v1, v2) => v1.Or(v2)));
}
