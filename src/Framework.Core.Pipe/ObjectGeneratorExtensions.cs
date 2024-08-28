namespace Framework.Core;

public static class ObjectGeneratorExtensions
{
    public static IEnumerable<T> GetAllElements<T>(this T source, Func<T, IEnumerable<T>> getChildFunc, bool skipFirstElement)
    {
        var baseElements = source.GetAllElements(getChildFunc);

        return skipFirstElement ? baseElements.Skip(1) : baseElements;
    }

    public static IEnumerable<T> GetAllElements<T>(this T source, Func<T, IEnumerable<T>> getChildFunc)
    {
        yield return source;

        foreach (var element in getChildFunc(source).SelectMany(child => child.GetAllElements(getChildFunc)))
        {
            yield return element;
        }
    }

    public static IEnumerable<T> GetAllElements<T>(this T? source, Func<T?, T> getNextFunc, bool skipFirstElement)
            where T : class
    {
        var baseElements = source.GetAllElements(getNextFunc);

        return skipFirstElement ? baseElements.Skip(1) : baseElements;
    }

    public static IEnumerable<T> GetAllElements<T>(this T? source, Func<T, T?> getNextFunc)
            where T : class
    {
        if (null == getNextFunc) throw new ArgumentNullException(nameof(getNextFunc));

        for (var state = source; state != null; state = getNextFunc(state))
        {
            yield return state;
        }
    }
}
