using System;
using System.Collections.Generic;

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

    public static TResult? Join<TOuter, TInner, TKey, TResult>(this TOuter? outer, TInner? inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
            where TOuter : struct
            where TInner : struct
            where TResult : struct
    {
        if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
        if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, null);
    }


    public static TResult? Join<TOuter, TInner, TKey, TResult>(this TOuter? outer, TInner? inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> preComparer)
            where TOuter : struct
            where TInner : struct
            where TResult : struct
    {
        if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
        if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return outer.ToMaybe().Join(inner.ToMaybe(), outerKeySelector, innerKeySelector, resultSelector, preComparer).ToNullable();
    }


    public static void Match<TSource>(this TSource? maybeValue, Action<TSource> fromJustAction, Action fromNothingAction = null)
            where TSource : struct
    {
        if (fromJustAction == null) throw new ArgumentNullException(nameof(fromJustAction));

        maybeValue.ToMaybe().Match(fromJustAction, fromNothingAction);
    }

    public static TResult Match<TSource, TResult>(this TSource? maybeValue, Func<TSource, TResult> fromJustResult, Func<TResult> fromNothingResult)
            where TSource : struct
    {
        if (fromJustResult == null) throw new ArgumentNullException(nameof(fromJustResult));
        if (fromNothingResult == null) throw new ArgumentNullException(nameof(fromNothingResult));

        return maybeValue.ToMaybe().Match(fromJustResult, fromNothingResult);
    }


    public static T? Or<T>(this T? v1, T? v2)
            where T : struct
    {
        return v1.ToMaybe().Or(v2.ToMaybe()).ToNullable();
    }

    public static T? Or<T>(this T? v1, Func<T> getV2)
            where T : struct
    {
        if (getV2 == null) throw new ArgumentNullException(nameof(getV2));

        return v1.ToMaybe().Or(getV2).ToNullable();
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
}
