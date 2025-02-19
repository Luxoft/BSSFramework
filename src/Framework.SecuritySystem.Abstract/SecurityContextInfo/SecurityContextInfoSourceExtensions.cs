namespace Framework.SecuritySystem;

public static class SecurityContextInfoSourceExtensions
{
    public static IEnumerable<Type> GetSecurityContextTypes(this ISecurityContextInfoSource securityContextInfoSource) =>
        securityContextInfoSource.SecurityContextInfoList.Select(info => info.Type);

    public static SecurityContextInfo GetSecurityContextInfo<TSecurityContext>(this ISecurityContextInfoSource securityContextInfoSource)
        where TSecurityContext : ISecurityContext
    {
        return securityContextInfoSource.GetSecurityContextInfo(typeof(TSecurityContext));
    }
}
