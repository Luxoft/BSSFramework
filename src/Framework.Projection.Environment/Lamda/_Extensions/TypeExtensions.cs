using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Core;

namespace Framework.Projection.Lambda;

internal static class TypeExtensions
{
    private static readonly IDictionaryCache<Type, IDictionaryCache<Type, Type>> CollectionTypeCache = new DictionaryCache<Type, IDictionaryCache<Type, Type>>(
     collectionType => new DictionaryCache<Type, Type>(collectionType.MakeProjectionCollectionType).WithLock()).WithLock();


    public static Type GetProjectionCollectionType(this Type type)
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
}
