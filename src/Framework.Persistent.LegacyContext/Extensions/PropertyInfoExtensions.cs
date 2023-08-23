using System.Reflection;

using Framework.Core;

namespace Framework.Persistent;

public static class PropertyInfoExtensions
{
    public static bool IsVisualIdentity(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasAttribute<VisualIdentityAttribute>();
    }

    public static bool IsMaster(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasAttribute<IsMasterAttribute>();
    }
}
