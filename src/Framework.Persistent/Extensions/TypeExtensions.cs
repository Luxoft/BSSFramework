using Framework.Core;

namespace Framework.Persistent;

public static class TypeExtensions
{
    internal static bool SafeEquals(this Type t1, Type t2)
    {
        if (t1 == null) throw new ArgumentNullException(nameof(t1));
        if (t2 == null) throw new ArgumentNullException(nameof(t2));

        return t1.IsGenericTypeDefinition == t2.IsGenericTypeDefinition && t1 == t2;
    }

    internal static bool SafeIsAssignableFrom(this Type t1, Type t2)
    {
        if (t1 == null) throw new ArgumentNullException(nameof(t1));
        if (t2 == null) throw new ArgumentNullException(nameof(t2));

        return t1.SafeEquals(t2) || t1.IsAssignableFrom(t2);
    }


    public static string ExtractSystemName(this Type type)
    {
        return type.Namespace!.Split('.').Skip("Framework", false).First();
    }
}
