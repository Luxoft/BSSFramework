using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.DomainDriven.BLLCoreGenerator;

internal static class PropertyInfoExtensions
{
    public static IEnumerable<PropertyInfo> GetInternalProperties(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   .Where(Core.PropertyInfoExtensions.HasFamilyGetMethod);
    }
}
