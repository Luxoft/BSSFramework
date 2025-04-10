using Framework.Core;

namespace Framework.SecuritySystem;

public static class SecurityAccessorDataExtensions
{
    public static SecurityAccessorData Or(this IEnumerable<SecurityAccessorData> source) =>
        source.Match(
            () => SecurityAccessorData.Empty,
            result => result,
            results => results.Aggregate(
                SecurityAccessorData.Empty,
                (v1, v2) => new SecurityAccessorData.OrSecurityAccessorData(v1, v2)));
}
