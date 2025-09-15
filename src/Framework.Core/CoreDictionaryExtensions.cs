namespace Framework.Core;

public static class CoreDictionaryExtensions
{
    public static Dictionary<TKey, TValue> Concat<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> other)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));

        return ((IEnumerable<KeyValuePair<TKey, TValue>>)source).Concat(other).ToDictionary();
    }
}
