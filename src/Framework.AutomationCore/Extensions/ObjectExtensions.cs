using System;
using System.ComponentModel;
using System.Reflection;

namespace Automation.Extensions;

public static class ObjectExtensions
{
    public static string GetDescription(this Enum obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperty(propertyName);
        var attr = propertyInfo?.GetCustomAttribute(typeof(DescriptionAttribute));

        return attr != null
            ? ((DescriptionAttribute)attr).Description
            : propertyName;
    }
}
