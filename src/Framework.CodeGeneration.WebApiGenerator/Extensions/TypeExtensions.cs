using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Serialization.Extensions;
using Framework.Core;
using Framework.Core.Helpers;
using Framework.Projection;

namespace Framework.CodeGeneration.WebApiGenerator.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<ViewDTOType> GetViewDTOTypes(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (type.IsProjection())
        {
            return [ViewDTOType.ProjectionDTO];
        }
        else
        {
            return from dtoType in EnumHelper.GetValues<ViewDTOType>()

                   where dtoType != ViewDTOType.VisualDTO || type.HasVisualIdentityProperties()

                   select dtoType;
        }
    }
}
