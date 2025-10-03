using CommonFramework.Maybe;

namespace Framework.Core;

public static class NullableExtensions
{
    public static TResult? Select<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToMaybe().Select(selector).ToNullable();
    }


    public static TResult? SelectMany<TSource, TNextResult, TResult>(this TSource? source, Func<TSource, TNextResult?> nextSelector, Func<TSource, TNextResult, TResult> resultSelector)
            where TSource : struct
            where TResult : struct
            where TNextResult : struct
    {
        if (nextSelector == null) throw new ArgumentNullException(nameof(nextSelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return source.ToMaybe().SelectMany(next => nextSelector(next).ToMaybe(), resultSelector).ToNullable();
    }



    public static T? Where<T>(this T? source, Func<T, bool> filter)
            where T : struct
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return source.ToMaybe().Where(filter).ToNullable();
    }

    public static T? Or<T>(this T? v1, T v2)
            where T : struct
    {
        return v1.ToMaybe().Or(v2).ToNullable();
    }

    public static T? Or<T>(this T? v1, Func<T?> getV2)
            where T : struct
    {
        return v1.ToMaybe().Or(() => getV2().ToMaybe()).ToNullable();
    }

    public static T? UnsafeOperation<T>(this T? v1, T? v2, Func<T, T, T> selector)
            where T : struct
    {
        return v1.HasValue ? v2.HasValue ? selector(v1.Value, v2.Value)
                                     : v1
                       : v2;
    }

    public static T? ToNullable<T>(this Maybe<T> value)
        where T : struct
    {
        return value.Match(v => v, () => default(T?));
    }
    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, TResult v2)
        where TSource : TResult
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));

        return v1.HasValue ? v1.Select(v => (TResult)v) : Maybe.Return(v2);
    }
}
