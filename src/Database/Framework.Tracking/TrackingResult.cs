using System.Collections.Concurrent;

using Framework.Database;

namespace Framework.Tracking;

/// <summary>
/// Internal helper that helps creation result
/// </summary>
internal static class TrackingResult
{
    /// <summary>
    /// Creates new TrackingResult instance using domain object specified
    /// </summary>
    /// <param name="source">Domain object</param>
    /// <param name="mode">Mode that defines changes detection algorithm on not persistent objects (that not exist in DB)</param>
    public static TrackingResult<TDomainObject> Create<TDomainObject>(IPersistentInfoService persistentInfoService, TDomainObject source, GetChangesMode mode)
    {
        var allProperties =
            typeof(TDomainObject).GetProperties()
                                 .Where(persistentInfoService.IsPersistent)
                                 .Select(z => new { Type = z.PropertyType, Value = z.GetValue(source, []), Name = z.Name })
                                 .Where(z => mode == GetChangesMode.Default || !Equals(z.Value, GetDefault(z.Type)))
                                 .Select(z => new TrackingProperty(z.Name, null, z.Value))
                                 .ToList();

        return new TrackingResult<TDomainObject>(allProperties);
    }

    private static object GetDefault(Type type)
    {
        return ThreadsafeMemoize<Type, object>(t => t.IsValueType ? Activator.CreateInstance(t) : null)(type);
    }

    private static Func<TArg, TResult> ThreadsafeMemoize<TArg, TResult>(this Func<TArg, TResult> func)
    {
        var cache = new ConcurrentDictionary<TArg, TResult>();

        return argument => cache.GetOrAdd(argument, func);
    }
}
