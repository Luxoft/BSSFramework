using System;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven;

internal static class FetchPropertyInfoExtensions
{
    public static MainDTOType GetMainDTOType(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.IsDetail()                  ? MainDTOType.RichDTO
               : property.IsNotDetail()               ? MainDTOType.SimpleDTO
               : property.PropertyType.IsCollection() ? MainDTOType.RichDTO
               : MainDTOType.SimpleDTO;
    }

    public static MainDTOType GetMainDTOType(this PropertyInfo property, MainDTOType maxDTOType)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetMainDTOType().Min(maxDTOType);
    }

    public static bool IsNotMaster(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var propertyType = property.PropertyType;

        return property.DeclaringType.GetProperties().Any(prop =>

                                                                  prop != property && prop.PropertyType == propertyType && prop.IsMaster());
    }
}
