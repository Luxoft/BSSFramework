using System.Diagnostics;

namespace Framework.Core;

public static class CoreObjectExtensions
{
    public static IEnumerable<int> RangeInfinity(this int startIndex)
    {
        while (true)
        {
            yield return startIndex++;
        }
    }

    public static bool IsDefault<T>(this T value)
    {
        return EqualityComparer<T>.Default.Equals(value, default(T));
    }

    public static TResult MaybeNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector, TResult nullableResult = default(TResult))
        where TSource : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? nullableResult : selector(source.Value);
    }
    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this TKey key, TValue value)
    {
        return new KeyValuePair<TKey, TValue>(key, value);
    }
}
