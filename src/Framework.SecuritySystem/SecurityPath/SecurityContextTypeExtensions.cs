namespace Framework.SecuritySystem;

public static class SecurityContextTypeExtensions
{
    public static bool IsSecurityContext(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return typeof(ISecurityContext).IsAssignableFrom(type);
    }
}
