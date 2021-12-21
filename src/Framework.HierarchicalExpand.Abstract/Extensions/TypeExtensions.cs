using System;
using System.Linq;

using Framework.Core;

namespace Framework.Persistent
{
    public static class TypeExtensions
    {
        public static bool IsHierarchicalDenormalized(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetHierarchicalAncestorLinkType() != null;
        }

        public static HierarchicalDenormalizeTypesDeclarations? GetHierarchicalAncestorLinkType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var denormalizedHierarchyType = type.GetInterfaces()
                                                .FirstOrDefault(
                                                    z => z.IsGenericType
                                                         && z.GetGenericTypeDefinition()
                                                         == typeof(IDenormalizedHierarchicalPersistentSource<,,,>));

            if (null == denormalizedHierarchyType)
            {
                return null;
            }

            var genericArguments = denormalizedHierarchyType.GetGenericArguments();

            return new HierarchicalDenormalizeTypesDeclarations(genericArguments[0], genericArguments[1]);
        }

        public static bool IsHierarchical(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsInterfaceImplementation(typeof(IHierarchicalPersistentDomainObjectBase<,>));
        }
    }
}
