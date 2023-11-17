namespace Framework.Core;

public static class DictionaryExtensions
{
    public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> getNewPairValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getNewPairValue == null) throw new ArgumentNullException(nameof(getNewPairValue));

        if (source.TryGetValue(key, out var value))
        {
            return value;
        }

        value = getNewPairValue();

        source.Add(key, value);

        return value;
    }

    public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, object syncLocker, Func<TValue> getNewPairValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (syncLocker == null) throw new ArgumentNullException(nameof(syncLocker));
        if (getNewPairValue == null) throw new ArgumentNullException(nameof(getNewPairValue));

        if (source.TryGetValue(key, out var value))
        {
            return value;
        }

        lock (syncLocker)
        {
            if (source.TryGetValue(key, out value))
            {
                return value;
            }

            value = getNewPairValue();

            source.Add(key, value);

            return value;
        }
    }

    public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TValue>> valueSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));

        var result = new Dictionary<TKey, TValue>();

        foreach (var item in source)
        {
            result.Add(keySelector(item), await valueSelector(item));
        }

        return result;
    }
}
