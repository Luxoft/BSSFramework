namespace Framework.SecuritySystem;

public static class SecurityContextSourceExtensions
{
    public static IEnumerable<Type> GetSecurityContextTypes(this ISecurityContextSource securityContextSource) =>
        securityContextSource.SecurityContextInfoList.Select(info => info.Type);

    public static SecurityContextInfo GetSecurityContextInfo<TSecurityContext>(this ISecurityContextSource securityContextSource)
        where TSecurityContext : ISecurityContext
    {
        return securityContextSource.GetSecurityContextInfo(typeof(TSecurityContext));
    }
}
