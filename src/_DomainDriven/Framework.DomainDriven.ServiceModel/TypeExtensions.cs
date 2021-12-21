using System;
using System.Linq;

namespace Framework.DomainDriven.ServiceModel
{
    public static class TypeExtensions
    {
        public static bool IsAccessableFrom (this Type source, params Type[] types)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (types == null) throw new ArgumentNullException(nameof(types));

            return types.Any(source.IsAssignableFrom);
        }
    }
}