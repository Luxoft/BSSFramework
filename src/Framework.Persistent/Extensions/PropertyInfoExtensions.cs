using System.Reflection;

using Framework.Core;

namespace Framework.Persistent;

public static class PropertyInfoExtensions
{
    public static bool IsDetail(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasDetailRole(true);
    }

    public static bool IsNotDetail(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasDetailRole(false);
    }

    public static bool HasDetailRole(this PropertyInfo propertyInfo, bool value)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var detailRoleAttr = propertyInfo.GetCustomAttribute<DetailRoleAttribute>(false)
                             ?? propertyInfo.DeclaringType.GetCustomAttribute<DetailRoleAttribute>(false);

        return detailRoleAttr.Maybe(attr => attr.HasValue(value));
    }

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
