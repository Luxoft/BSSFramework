using System.Diagnostics;

using CommonFramework.Maybe;

namespace Framework.Core;

using CommonFramework;

public static class CorePipeMaybeObjectExtensions
{
    public static TResult IfDefault<TResult>(this TResult source, TResult otherResult)
    {
        return source.IsDefault() ? otherResult : source;
    }

    public static TSource PipeMaybe<TSource, TValue>(this TSource source, TValue? value, Func<TSource, TValue, TSource> evaluate)
        where TValue : struct
    {
        return value == null ? source : evaluate(source, value.Value);
    }

    public static void Match<TSource>(this Maybe<TSource> maybeValue, Action<TSource> fromJustAction, Action? fromNothingAction = null)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (fromJustAction == null) throw new ArgumentNullException(nameof(fromJustAction));

        var just = maybeValue as Just<TSource>;

        if (just == null)
        {
            fromNothingAction?.Invoke();
        }
        else
        {
            fromJustAction(just.Value);
        }
    }

    public static TSource FromMaybe<TSource>(this TSource source, string nothingExceptionMessage)
        where TSource : class
    {
        return source.FromMaybe(() => nothingExceptionMessage);
    }

    [DebuggerStepThrough]
    public static TResult? MaybeToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
        where TSource : class
        where TResult : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? default(TResult?) : selector(source);
    }

    public static TResult? MaybeNullableToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
        where TSource : struct
        where TResult : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? default(TResult?) : selector(source.Value);
    }
}
