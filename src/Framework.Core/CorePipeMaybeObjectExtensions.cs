using System.Diagnostics;

namespace Framework.Core;

public static class CorePipeMaybeObjectExtensions
{
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
