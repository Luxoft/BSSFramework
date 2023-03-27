namespace Framework.Core;

public static class DictionaryExtensions
{
    public static Maybe<TValue> GetMaybeValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.TryGetValue(key, out var value) ? new Just<TValue>(value) : Maybe<TValue>.Nothing;
    }
}
