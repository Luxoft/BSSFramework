using System.Reflection;

using Framework.Application.Domain.Attributes;
using Framework.BLL.Domain.Dto;
using Framework.BLL.Domain.Dto.Extensions;
using Framework.BLL.Domain.Extensions;
using Framework.Core;

namespace Framework.BLL.Fetching.PathFactory._Extensions;

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
