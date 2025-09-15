namespace Framework.Core;

public static class CoreDictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<TValue> getDefaultValueFunc)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getDefaultValueFunc == null) throw new ArgumentNullException(nameof(getDefaultValueFunc));

        TValue value;

        return source.TryGetValue(key, out value) ? value : getDefaultValueFunc();
    }

    public static Dictionary<TKey, TValue> Concat<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> other)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));

        return ((IEnumerable<KeyValuePair<TKey, TValue>>)source).Concat(other).ToDictionary();
    }
}
