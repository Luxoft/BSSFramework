using System.Diagnostics;

namespace Framework.Core;

public static class PipeMaybeObjectExtensions
{
    [DebuggerStepThrough]
    public static void Maybe<TSource>(this TSource? source, Action<TSource> evaluate)
            where TSource : class
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        if (null != source)
        {
            evaluate(source);
        }
    }

    [DebuggerStepThrough]
    public static TResult? Maybe<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
            where TSource : class
    {
        return null == source ? default(TResult) : selector(source);
    }


    public static void MaybeNullable<TSource>(this TSource? source, Action<TSource> evaluate)
            where TSource : struct
    {
        if (null != source)
        {
            evaluate(source.Value);
        }
    }

    public static TResult MaybeNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector, TResult nullableResult = default(TResult))
            where TSource : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? nullableResult : selector(source.Value);
    }

    public static TResult? MaybeNullableToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? default(TResult?) : selector(source.Value);
    }

    public static TResult? Maybe<TSource, TResult>(this TSource? source, Func<TSource, bool> condition, Func<TSource, TResult> getResult)
            where TSource : class
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        if (getResult == null) throw new ArgumentNullException(nameof(getResult));

        return source.Maybe(v => condition(v) ? getResult(v) : default(TResult));
    }

    [DebuggerStepThrough]
    public static TResult? MaybeToNullable<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
            where TSource : class
            where TResult : struct
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? default(TResult?) : selector(source);
    }

    [DebuggerStepThrough]
    public static TResult Maybe<TObject, TResult>(this TObject? source, Func<TObject, TResult> selector, TResult ifNullResult)
            where TObject : class
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? ifNullResult : selector(source);
    }

    [DebuggerStepThrough]
    public static TResult Maybe<TObject, TResult>(this TObject? source, Func<TObject, TResult> selector, Func<TResult> ifNullResult)
            where TObject : class
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return null == source ? ifNullResult() : selector(source);
    }

    public static void Maybe<TObject>(this TObject? source, Func<TObject, bool> condition, Action<TObject> action)
            where TObject : class
    {
        if (null == source)
        {
            return;
        }
        if (condition(source)) action(source);
    }
}
