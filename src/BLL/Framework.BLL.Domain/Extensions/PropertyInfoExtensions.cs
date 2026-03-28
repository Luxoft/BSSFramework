using System.Reflection;

using Framework.BLL.Domain.Attributes;
using Framework.Core;

namespace Framework.BLL.Domain.Extensions;

public static class PropertyInfoExtensions
{
    public static bool IsVisualIdentity(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasAttribute<VisualIdentityAttribute>();
    }
}
