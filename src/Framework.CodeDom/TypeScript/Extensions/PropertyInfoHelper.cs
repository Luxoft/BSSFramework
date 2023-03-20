using System;
using System.Reflection;

using Framework.Core;

namespace Framework.CodeDom.TypeScript;

/// <summary>
/// Property info helper extensions
/// </summary>
public static class PropertyInfoExtensions
{
    public static bool IsDateTime(this PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;

        return IsDateTime(type);
    }

    public static bool IsPeriod(this PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;

        return IsPeriod(type);
    }

    public static bool IsGuid(this PropertyInfo propertyInfo)
    {
        var type = propertyInfo.PropertyType;
        return IsGuid(type);
    }

    public static bool IsDateTime(this Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    public static bool IsPeriod(this Type type)
    {
        return type == typeof(Period) || type == typeof(Period?);
    }

    public static bool IsGuid(this Type type)
    {
        return type == typeof(Guid) || type == typeof(Guid?);
    }

    public static bool IsPrimitiveJsType(this PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.IsPrimitiveJsType();
    }

    public static bool IsPrimitiveJsType(this Type value)
    {
        var nullableType = value.GetNullableElementType();

        if (nullableType != null)
        {
            return nullableType.IsPrimitiveJsType();
        }

        var result = typeof(Guid) == value
                     || value == typeof(DateTime)
                     || value == typeof(Period)
                     || value == typeof(TimeSpan)
                     || value.IsEnum
                     || value.IsPrimitive
                     || value == typeof(string)
                     || value == typeof(decimal)
                     || value == typeof(byte);

        return result;
    }

    public static string GetBasedOutputType(this Type type)
    {
        return GetBasedOutputType(type.FullName);
    }

    public static string GetBasedOutputType(this string baseType)
    {
        if (baseType.Length == 0)
        {
            return "void";
        }

        if (string.Compare(baseType, "System.Byte", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Int16", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Int32", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Int64", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.SByte", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.UInt16", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.UInt32", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.UInt64", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Decimal", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Single", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Double", StringComparison.Ordinal) == 0)
        {
            return "number";
        }

        if (string.Compare(baseType, "System.Boolean", StringComparison.Ordinal) == 0)
        {
            return "boolean";
        }

        if (string.Compare(baseType, "System.Char", StringComparison.Ordinal) == 0)
        {
            return "string";
        }

        if (string.Compare(baseType, "System.DateTime", StringComparison.Ordinal) == 0)
        {
            return "Date";
        }

        if (string.Compare(baseType, "System.String", StringComparison.Ordinal) == 0)
        {
            return "string";
        }

        if (string.Compare(baseType, "System.Void", StringComparison.Ordinal) == 0)
        {
            return "void";
        }

        if (string.Compare(baseType, "System.Guid", StringComparison.Ordinal) == 0)
        {
            return "Guid";
        }

        return baseType;
    }
}
