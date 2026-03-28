using System.Reflection;

using Framework.Core;

namespace Framework.Relations;

public static class DetailRoleExtensions
{
    public static bool IsDetail(this PropertyInfo propertyInfo) => propertyInfo.HasDetailRole(true);

    public static bool IsNotDetail(this PropertyInfo propertyInfo) => propertyInfo.HasDetailRole(false);

    public static bool HasDetailRole(this PropertyInfo propertyInfo, bool value) =>
        (propertyInfo.GetCustomAttribute<DetailRoleAttribute>(false)
         ?? propertyInfo.DeclaringType!.GetCustomAttribute<DetailRoleAttribute>(false))
        is { } detailRoleAttr
        && detailRoleAttr.HasValue(value);

    public static bool IsMaster(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasAttribute<IsMasterAttribute>();
    }
}
