namespace Framework.Core.TypeResolving;

public static class TypeSearchModeExtensions
{
    public static Func<Type, string, bool> ToFilter(this TypeSearchMode searchMode)
    {
        var filters = searchMode.ToFilters().ToArray();

        return (type, ident) => filters.Any(f => f(type, ident));
    }

    private static IEnumerable<Func<Type, string, bool>> ToFilters(this TypeSearchMode searchMode)
    {
        if (searchMode.HasFlag(TypeSearchMode.Name))
        {
            yield return (type, name) => type.Name == name;
        }

        if (searchMode.HasFlag(TypeSearchMode.FullName))
        {
            yield return (type, fullName) => type.FullName == fullName;
        }
    }
}
