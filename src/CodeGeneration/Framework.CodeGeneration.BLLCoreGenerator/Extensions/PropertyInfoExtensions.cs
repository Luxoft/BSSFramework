using System.Reflection;

using Framework.Core;

namespace Framework.CodeGeneration.BLLCoreGenerator.Extensions;

internal static class PropertyInfoExtensions
{
    public static IEnumerable<PropertyInfo> GetInternalProperties(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   .Where(CorePropertyInfoExtensions.HasFamilyGetMethod);
    }
}
