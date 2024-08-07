using System.Reflection;

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

    public static IEnumerable<Type> GetDetailTypes(this Type type)
    {
        return type.GetInterfaces()
                   .Select(i => i.GetInterfaceImplementationArgument(typeof(IMaster<>)))
                   .Where(detailType => detailType != null);
    }

    public static IEnumerable<PropertyInfo> GetDetailProperties(this Type type)
    {
        return from detailType in type.GetDetailTypes()

               join property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) on detailType equals property.PropertyType.GetCollectionElementType() into propertyGroup

               select propertyGroup.Count() == 1 ? propertyGroup.Single() : propertyGroup.Single(property => property.HasPrivateField());
    }

    public static string ExtractSystemName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Namespace.Split('.').Skip("Framework", false).First();
    }
}
