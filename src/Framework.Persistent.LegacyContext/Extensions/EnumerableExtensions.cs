namespace Framework.Persistent;

public static class EnumerableExtensions
{
    public static TSource GetByName<TSource>(this IEnumerable<TSource> source, string name, bool raiseIfNotFound = true)
            where TSource : class, IVisualIdentityObject
    {
        return source.GetByName(name, StringComparison.CurrentCultureIgnoreCase, raiseIfNotFound);
    }

    public static TSource GetByName<TSource>(this IEnumerable<TSource> source, string name, Func<Exception> getFaultException)
            where TSource : class, IVisualIdentityObject
    {
        return source.GetByName(name, StringComparison.CurrentCultureIgnoreCase, getFaultException);
    }


    public static TSource GetByName<TSource>(this IEnumerable<TSource> source, string name, StringComparison stringComparison, bool raiseIfNotFound = true)
            where TSource : class, IVisualIdentityObject
    {
        return source.GetByName(name, stringComparison, TryGetNotFoundByNameExceptionFactory<TSource>(name, raiseIfNotFound));
    }

    public static TSource GetByName<TSource>(this IEnumerable<TSource> source, string name, StringComparison stringComparison, Func<Exception> getFaultException)
            where TSource : class, IVisualIdentityObject
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (name == null) throw new ArgumentNullException(nameof(name));

        var res = source.SingleOrDefault(obj => string.Equals(obj.Name, name, stringComparison));

        if (res == null && getFaultException != null)
        {
            throw getFaultException();
        }

        return res;
    }


    private static Func<Exception> TryGetNotFoundByNameExceptionFactory<TSource>(string name, bool raiseIfNotFound)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return raiseIfNotFound ? () => GetNotFoundException<TSource>(name, nameof(name)) : default(Func<Exception>);
    }

    private static Exception GetNotFoundException<TSource>(string identity, string identityName)
    {
        throw new Exception($"{typeof (TSource).Name} with {identityName} \"{identity}\" not found");
    }
}
