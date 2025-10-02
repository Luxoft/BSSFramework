using CommonFramework.DictionaryCache;

namespace Framework.Core;

public static class CoreDictionaryExtensions
{
    public static Dictionary<TNewKey, TValue> ChangeKey<TOldKey, TNewKey, TValue>(this IReadOnlyDictionary<TOldKey, TValue> source, Func<TOldKey, TNewKey> selector)
        where TNewKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToDictionary(pair => selector(pair.Key), pair => pair.Value);
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

    public static TValue GetValue<TK1, TK2, TK3, TValue>(this IDictionaryCache<Tuple<TK1, TK2, TK3>, TValue> cache, TK1 tk1, TK2 tk2, TK3 tk3)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache[Tuple.Create(tk1, tk2, tk3)];
    }


    public static Dictionary<TKey, TValue> Concat<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> other)
        where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));

        return ((IEnumerable<KeyValuePair<TKey, TValue>>)source).Concat(other).ToDictionary();
    }

    public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.ContainsKey(key))
        {
            source[key] = value;
        }
        else
        {
            source.Add(key, value);
        }
    }
}
