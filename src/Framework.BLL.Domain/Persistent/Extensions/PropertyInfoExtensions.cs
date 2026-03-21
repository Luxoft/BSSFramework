using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Domain.Persistent.Extensions;

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
                             ?? propertyInfo.DeclaringType!.GetCustomAttribute<DetailRoleAttribute>(false);

        return detailRoleAttr.Maybe(attr => attr.HasValue(value));
    }
}
