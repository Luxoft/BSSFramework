using System.Reflection;

using Framework.Core;

namespace Framework.Persistent.Mapping;

public static class PropertyInfoExtensions
{
    /// <summary>
    /// Проверка свойства на персистентность
    /// </summary>
    /// <param name="propertyInfo">Свойство</param>
    /// <returns></returns>
    public static bool IsPersistent(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasPrivateField(true)
               || propertyInfo.GetPrivateField().Maybe(field => !field.HasAttribute<NotPersistentFieldAttribute>());
    }
}
