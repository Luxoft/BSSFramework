using System.Collections.ObjectModel;

namespace Framework.Core;

public static class TypeExtensions
{
    private static readonly HashSet<Type> CollectionTypes = new[]
                                                            {
                                                                    typeof(IEnumerable<>),
                                                                    typeof(List<>),
                                                                    typeof(Collection<>),
                                                                    typeof(IList<>),
                                                                    typeof(ICollection<>),
                                                                    typeof(ObservableCollection<>),
                                                                    typeof(IReadOnlyList<>),
                                                                    typeof(IReadOnlyCollection<>)
                                                            }.ToHashSet();

    public static Type GetCollectionElementTypeOrSelf(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionElementType() ?? type;
    }

    public static Type? GetCollectionElementType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionType() != null ? type.GetGenericArguments().Single() : null;
    }

    public static Type? GetCollectionType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();

            if (CollectionTypes.Contains(genericType))
            {
                return genericType;
            }
        }

        return null;
    }

    public static Type? GetArrayGenericType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsArray ? type.GetElementType() : null;
    }

    public static Type? GetCollectionOrArrayElementType(this Type type)
    {
        return type.GetCollectionElementType() ?? type.GetArrayGenericType();
    }

    public static Type GetCollectionOrArrayElementTypeOrSelf(this Type type)
    {
        return type.GetCollectionOrArrayElementType() ?? type;
    }

    public static bool IsCollection(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionElementType() != null;
    }

    public static bool IsCollectionOrArray(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionOrArrayElementType() != null;
    }

    public static bool IsCollection(this Type type, Func<Type?, bool> elementTypeFilter)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (elementTypeFilter == null) throw new ArgumentNullException(nameof(elementTypeFilter));

        return type.IsCollection() && elementTypeFilter(type.GetCollectionElementType());
    }

    public static Type GetEnumerableElementType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetInterfaceImplementationArguments(typeof(IEnumerable<>), args => args.Single());
    }
}
