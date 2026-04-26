using System.Reflection;

using Anch.Core;

namespace Framework.Validation;

public static class TypeExtensions
{
    public static string GetValidationName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCustomAttribute<CustomNameAttribute>().Maybe(v => v.Name) ?? type.Name;
    }

    public static string GetValidationName(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetCustomAttribute<CustomNameAttribute>().Maybe(v => v.Name) ?? property.Name;
    }
}
