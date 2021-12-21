using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.Metadata
{
    public static class TypeExtensions2
    {
        public static bool IsPrimitiveExt(this Type source)
        {
            if (source.IsGenericType)
            {
                return (source.GetGenericTypeDefinition() == typeof(Nullable<>)) && source.GetGenericArguments()[0].IsPrimitiveExt();
            }

            var logicPrimitiveTypes = new[]
                                      {
                                          typeof(string),
                                          typeof(Guid),
                                          typeof(DateTime),
                                          typeof(decimal),
                                          typeof(int),
                                          typeof(byte[])
                                      };
            return source.IsPrimitive
                   || logicPrimitiveTypes.Contains(source)
                   || source.IsEnum;
        }

        public static bool IsInlineType(this Type type, Type baseDomainType) =>
                !type.IsPrimitiveExt()
                && !type.IsDomainType(baseDomainType)
                && !type.IsDomainTypeList(baseDomainType)
                && !typeof(IEnumerable).IsAssignableFrom(type);

        public static bool IsDomainTypeList(this Type type, Type baseDomainType) =>
                type.IsGenericType
                && type.GetGenericTypeDefinition().IsInterfaceImplementation(typeof(ICollection<>))
                && type.GetGenericArguments()[0].IsAssignableConcreteType(baseDomainType);

        private static bool IsAssignableConcreteType(this Type type, Type baseDomainType) => !type.IsAbstract && type.IsDomainType(baseDomainType);

        public static bool IsDomainType(this Type type, Type baseDomainType) => baseDomainType.IsAssignableFrom(type);
    }
}
