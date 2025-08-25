using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Persistent;

namespace Framework.Validation;

public static class PropertyInfoExtensions
{
    public static string GetUniqueElementString(this IEnumerable<PropertyInfo> properties, bool withBrackets)
    {
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        var body = properties.Select(prop => prop.GetValidationName()).Join(", ");

        return withBrackets ? $"({body})" : body;
    }

    public static bool HasDeepValidation(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var modeAttr = property.GetCustomAttribute<PropertyValidationModeAttribute>();

        if (modeAttr != null)
        {
            if (modeAttr.Mode == PropertyValidationMode.Disabled)
            {
                return false;
            }

            switch (modeAttr.DeepMode)
            {
                case PropertyValidationMode.Disabled:
                    return false;

                case PropertyValidationMode.Enabled:
                    return true;
            }
        }

        if (property.PropertyType.IsCollection())
        {
            return !property.IsNotDetail();
        }
        else
        {
            return property.IsDetail() || property.PropertyType.HasExpandValidation();
        }
    }
}
