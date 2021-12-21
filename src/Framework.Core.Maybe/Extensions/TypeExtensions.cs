using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Framework.Core
{
    [DataContract]
    public static class TypeExtensions
    {
        public static Type GetMaybeElementType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsGenericTypeImplementation(typeof(Maybe<>)) ? type.GetGenericArguments().Single() : null;
        }

        public static Type GetMaybeElementTypeOrSelf(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetMaybeElementType() ?? type;
        }

        public static bool IsMaybe(this Type type, bool withNested = false)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetMaybeElementType() != null

                   || (withNested && (type.IsGenericTypeImplementation(typeof(Just<>)) || type.IsGenericTypeImplementation(typeof(Nothing<>))));
        }
    }
}
