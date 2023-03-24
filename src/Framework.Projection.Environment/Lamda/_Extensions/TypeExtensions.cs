using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal static class TypeExtensions
{
    private static readonly IDictionaryCache<Type, IDictionaryCache<Type, Type>> CollectionTypeCache = new DictionaryCache<Type, IDictionaryCache<Type, Type>>(
     collectionType => new DictionaryCache<Type, Type>(collectionType.MakeProjectionCollectionType).WithLock()).WithLock();


    public static Type GetProjectionCollectionType([NotNull] this Type type)
    {
        if (type == null) { throw new ArgumentNullException(nameof(type)); }

        return type.IsCollection() ? type.GetCollectionType()
               : type.IsArray && !type.GetElementType().IsPrimitive ? typeof(Array)
               : null;
    }

    public static Type SafeMakeProjectionCollectionType(this Type collectionType, Type elementType)
    {
        if (elementType == null) { throw new ArgumentNullException(nameof(elementType)); }

        if (collectionType != null)
        {
            return CollectionTypeCache[collectionType][elementType];
        }
        else
        {
            return elementType;
        }
    }

    private static Type MakeProjectionCollectionType(this Type collectionType, Type elementType)
    {
        if (collectionType == null) throw new ArgumentNullException(nameof(collectionType));
        if (elementType == null) { throw new ArgumentNullException(nameof(elementType)); }

        if (collectionType == typeof(Array))
        {
            return elementType.MakeArrayType();
        }
        else if (collectionType.IsCollection())
        {
            return collectionType.CachedMakeGenericType(elementType);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(collectionType));
        }
    }

    /// <summary>
    /// Получение связанных типов для денормализации и иерархии
    /// </summary>
    /// <param name="sourceType">Базовый тип</param>
    /// <returns></returns>
    internal static IEnumerable<Type> GetHierarchicalSecurityTypes(this Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        if (sourceType.IsHierarchical())
        {
            var denormalizedHierarchicalArgs = sourceType.GetInterfaceImplementationArguments(typeof(IDenormalizedHierarchicalPersistentSource<,,,>));

            if (denormalizedHierarchicalArgs != null)
            {
                var denormalizedType = denormalizedHierarchicalArgs[0];

                var ancestorChildLinkType = denormalizedHierarchicalArgs[1];

                yield return denormalizedType;

                yield return ancestorChildLinkType;
            }
        }
    }

    /// <summary>
    /// Получение дополнительного имлементированного интерфейса для безопасности
    /// </summary>
    /// <param name="sourceType">Базовый тип</param>
    /// <returns></returns>
    internal static Type GetExtraSecurityNodeInterface(this Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        var extraGenericSecurityTypes = new[]
                                        {
                                                typeof(IDenormalizedHierarchicalPersistentSource<,,,>),
                                                typeof(IHierarchicalSource<>),
                                                typeof(IHierarchicalAncestorLink<,,>),
                                                typeof(IHierarchicalToAncestorOrChildLink<,>)
                                        };

        foreach (var interfaceType in extraGenericSecurityTypes)
        {
            if (sourceType.IsInterfaceImplementation(interfaceType))
            {
                return sourceType.GetInterfaceImplementation(interfaceType);
            }
        }

        return null;
    }
}
