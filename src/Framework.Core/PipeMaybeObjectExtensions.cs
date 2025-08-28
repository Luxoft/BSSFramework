using System.Diagnostics;

namespace Framework.Core;

public static class PipeMaybeObjectExtensions
{
    [DebuggerStepThrough]
    public static TResult? MaybeToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
            where TSource : class
            where TResult : struct
    {
        throw new Exception("Use CommonFramework");
    }

    public static TResult? MaybeNullableToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
        where TSource : struct
        where TResult : struct
    {
        throw new Exception("Use CommonFramework");
    }
}
