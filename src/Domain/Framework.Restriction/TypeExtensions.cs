using System.Reflection;

using Anch.Core;

using Framework.Core;

namespace Framework.Restriction;

public static class TypeExtensions2
{
    public static IEnumerable<PropertyInfo> GetUniqueElementProperties(this Type type, string groupKey)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return from elementProperty in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

               let getElementPropertyMethod = elementProperty.GetGetMethod()

               where getElementPropertyMethod != null
                     && elementProperty.HasAttribute<UniqueElementAttribute>(attr => string.Equals(groupKey, attr.Key, StringComparison.CurrentCultureIgnoreCase))

               select elementProperty;
    }

    public static PropertyInfo[] GetUniqueElementProperties(this Type type, string groupKey, bool checkAnyElements)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var uniProperties = type.GetUniqueElementProperties(groupKey).ToArray();

        if (!uniProperties.Any() && checkAnyElements)
        {
            throw new Exception($"No required group elements for key \"{groupKey}\" in domainType \"{type.Name}\"");
        }

        return uniProperties;
    }

    public static PropertyInfo[] GetUniqueElementProperties(this PropertyInfo property, string groupKey, bool checkAnyElements)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var elementType = property.PropertyType.GetCollectionElementType()!;

        var uniProperties = elementType.GetUniqueElementProperties(groupKey).ToArray();

        if (!uniProperties.Any() && checkAnyElements)
        {
            throw new Exception($"No group elements for key \"{groupKey}\" for property \"{property.Name}\" in domainType \"{property.ReflectedType.Name}\"");
        }

        return uniProperties;
    }

}
