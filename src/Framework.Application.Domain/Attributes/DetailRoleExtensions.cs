using System.Reflection;

namespace Framework.Application.Domain.Attributes;

public static class DetailRoleExtensions
{
    public static bool IsDetail(this PropertyInfo propertyInfo) => propertyInfo.HasDetailRole(true);

    public static bool IsNotDetail(this PropertyInfo propertyInfo) => propertyInfo.HasDetailRole(false);

    public static bool HasDetailRole(this PropertyInfo propertyInfo, bool value) =>
        (propertyInfo.GetCustomAttribute<DetailRoleAttribute>(false)
         ?? propertyInfo.DeclaringType!.GetCustomAttribute<DetailRoleAttribute>(false))
        is { } detailRoleAttr
        && detailRoleAttr.HasValue(value);
}
