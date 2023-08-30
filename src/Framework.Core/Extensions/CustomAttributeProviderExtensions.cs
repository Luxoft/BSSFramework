using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.Core;

public static class CustomAttributeProviderExtensions
{
    private static class AttributeCacheContainer<T>
            where T : Attribute
    {
        public static readonly IDictionaryCache<ICustomAttributeProvider, ReadOnlyCollection<T>> TypeAttributeCache = new DictionaryCache<ICustomAttributeProvider, ReadOnlyCollection<T>>(source =>
        {
            var preAttr = (source as PropertyInfo).Maybe(v => Attribute.GetCustomAttributes(v, typeof(T), true))
                          ?? source.GetCustomAttributes(typeof(T), true);

            return preAttr.Cast<T>().ToReadOnlyCollection();
        }).WithLock();
    }




    public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider source, Func<T, bool> predicate)
            where T : Attribute
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return AttributeCacheContainer<T>.TypeAttributeCache[source].Where(predicate);
    }


    public static T GetCustomAttribute<T>(this ICustomAttributeProvider source, bool throwIfMany = true)
            where T : Attribute
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetCustomAttribute<T>(_ => true, throwIfMany);
    }

    public static T GetCustomAttribute<T>(this ICustomAttributeProvider source, Func<T, bool> predicate, bool throwIfMany = true)
            where T : Attribute
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var attributes = source.GetCustomAttributes<T>();

        return throwIfMany ? attributes.SingleOrDefault(predicate, () => new Exception("Multiple defined attribute"))
                       : attributes.FirstOrDefault();
    }

    public static IEnumerable<T> GetCustomAttributes<T> (this ICustomAttributeProvider source)
            where T : Attribute
    {
        return source.GetCustomAttributes<T>(_ => true);
    }


    public static bool HasAttribute<T>(this ICustomAttributeProvider source)
            where T : Attribute
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetCustomAttributes<T>().Any();
    }

    public static bool HasAttribute(this ICustomAttributeProvider source, Func<Attribute, bool> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return source.GetCustomAttributes<Attribute>().Any(predicate);
    }

    public static bool HasAttribute<T>(this ICustomAttributeProvider source, Func<T, bool> predicate)
            where T : Attribute
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return source.GetCustomAttributes<T>().Any(predicate);
    }
}
