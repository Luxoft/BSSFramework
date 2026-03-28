using System.Reflection;

using CommonFramework;

using Framework.Core;

namespace Framework.Validation;

public static class TypeExtensions
{
    /// <summary>
    /// Проверка типа на глубокую комопзитную валидацию
    /// </summary>
    /// <param name="type">Тип</param>
    /// <returns></returns>
    public static bool HasExpandValidation(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.HasAttribute<ExpandValidationAttribute>();
    }

    public static IEnumerable<PropertyInfo> GetValidationProperties(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

               let getMethod = property.GetGetMethod()

               where getMethod != null && !property.GetIndexParameters().Any()
                                       && !property.HasAttribute<PropertyValidationModeAttribute>(attr => attr.HasValue(false))

               orderby property.PropertyType.IsCollection()

               select property;
    }

    internal static bool IsSystemOrCoreType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsSystemType() || type.IsCoreType();
    }

    internal static bool IsSystemType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Assembly == typeof(object).Assembly;
    }

    internal static bool IsCoreType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Assembly == typeof(Ignore).Assembly;
    }
}
