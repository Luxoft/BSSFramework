using System;
using System.Linq;

namespace Framework.Core
{
    public static class TypeExtensions
    {
        public static Type GetNullableObjectElementType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsGenericTypeImplementation(typeof(NullableObject<>)) ? type.GetGenericArguments().Single() : null;
        }

        public static Type GetNullableObjectElementTypeOrSelf(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetNullableObjectElementType() ?? type;
        }

        public static bool IsNullableObject(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetNullableObjectElementType() != null;
        }
    }
}
