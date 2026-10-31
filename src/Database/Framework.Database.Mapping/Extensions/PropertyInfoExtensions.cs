using System.Reflection;

using Anch.Core;

using Framework.Core;

namespace Framework.Database.Mapping.Extensions;

public static class PropertyInfoExtensions
{
    /// <summary>
    /// Check a property for persistence
    /// </summary>
    /// <param name="propertyInfo">Property</param>
    /// <returns></returns>
    public static bool IsPersistent(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetPrivateField().Maybe(field => !field.HasAttribute<NotPersistentFieldAttribute>());
    }
}
