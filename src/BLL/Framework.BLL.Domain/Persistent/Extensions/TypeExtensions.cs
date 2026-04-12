namespace Framework.BLL.Domain.Persistent.Extensions;

public static class TypeExtensions
{
    internal static bool SafeEquals(this Type t1, Type t2) => t1.IsGenericTypeDefinition == t2.IsGenericTypeDefinition && t1 == t2;

    internal static bool SafeIsAssignableFrom(this Type t1, Type t2) => t1.SafeEquals(t2) || t1.IsAssignableFrom(t2);
}
