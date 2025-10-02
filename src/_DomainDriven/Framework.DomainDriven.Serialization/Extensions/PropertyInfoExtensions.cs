using System.Reflection;

using CommonFramework.Maybe;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.Serialization;

public static class PropertyInfoExtensions
{
    private static Maybe<CustomSerializationMode> GetMode(this ICustomAttributeProvider customAttributeProvider, DTORole dtoRole)
    {
        if (customAttributeProvider == null) throw new ArgumentNullException(nameof(customAttributeProvider));

        return customAttributeProvider.GetCustomAttributes<CustomSerializationAttribute>()
                                      .FirstOrDefault(attr => attr.Role.HasFlag(dtoRole))
                                      .ToMaybe()
                                      .Select(attr => attr.Mode);
    }

    private static bool HasMode(this PropertyInfo propertyInfo, DTORole dtoRole, CustomSerializationMode mode)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetMode(dtoRole).Or(() => propertyInfo.DeclaringType.GetMode(dtoRole))
                           .Or(CustomSerializationMode.Normal)
                           .Select(attrMode => attrMode == mode)
                           .GetValueOrDefault();
    }



    public static bool IsReadable(this PropertyInfo propertyInfo, DTORole dtoRole)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return !propertyInfo.IsIgnored(dtoRole);
    }

    public static bool IsIgnored(this PropertyInfo propertyInfo, DTORole dtoRole)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasMode(dtoRole, CustomSerializationMode.Ignore);
    }


    public static bool IsReadOnly(this PropertyInfo propertyInfo, DTORole dtoRole)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasMode(dtoRole, CustomSerializationMode.ReadOnly);
    }

    public static bool IsFixReference(this PropertyInfo propertyInfo, DTORole dtoRole)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasMode(dtoRole, CustomSerializationMode.FixReference);
    }


    public static bool IsWritable(this PropertyInfo propertyInfo, DTORole dtoRole, bool allowHierarchical)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return !propertyInfo.IsIgnored(dtoRole)
               && !propertyInfo.IsReadOnly(dtoRole)
               && (propertyInfo.HasSetMethod() ||

                   propertyInfo.PropertyType.IsCollection(elementType => allowHierarchical || elementType != propertyInfo.DeclaringType || propertyInfo.IsDetail()));
    }
}
