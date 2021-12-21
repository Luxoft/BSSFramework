using System;
using System.Linq;

namespace Framework.Core
{
    public static class TypeExtensions
    {
        public static Type GetNullableElementType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsGenericTypeImplementation(typeof(Nullable<>)) ? type.GetGenericArguments().Single() : null;
        }

        public static Type GetNullableElementTypeOrSelf(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetNullableElementType() ?? type;
        }

        public static bool IsNullable(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetNullableElementType() != null;
        }
    }
}
