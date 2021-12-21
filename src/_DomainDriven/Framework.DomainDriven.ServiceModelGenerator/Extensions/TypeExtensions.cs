using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public static class TypeExtensions
    {
        public static IEnumerable<ViewDTOType> GetViewDTOTypes(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.IsProjection())
            {
                return new[] { ViewDTOType.ProjectionDTO };
            }
            else
            {
                return from dtoType in EnumHelper.GetValues<ViewDTOType>()

                       where dtoType != ViewDTOType.VisualDTO || type.HasVisualIdentityProperties()

                       select dtoType;
            }
        }
    }
}