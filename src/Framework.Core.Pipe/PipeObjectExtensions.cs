#nullable enable

namespace Framework.Core;

public static class PipeObjectExtensions
{
    public static TSource Self<TSource>(this TSource source, bool condition, Action<TSource> evaluate)
    {
        if (condition)
        {
            evaluate(source);
        }

        return source;
    }

    public static TSource Self<TSource>(this TSource source, Action<TSource> evaluate)
    {
        evaluate(source);

        return source;
    }

    public static void Pipe<T1, T2>(this Tuple<T1, T2> source, Action<T1, T2> evaluate)
    {
        evaluate(source.Item1, source.Item2);
    }

    public static TResult Pipe<T1, T2, TResult>(this Tuple<T1, T2> source, Func<T1, T2, TResult> evaluate)
    {
        return evaluate(source.Item1, source.Item2);
    }

    public static void Pipe<T1, T2, T3>(this Tuple<T1, T2, T3> source, Action<T1, T2, T3> evaluate)
    {
        evaluate(source.Item1, source.Item2, source.Item3);
    }

    public static TResult Pipe<T1, T2, T3, TResult>(this Tuple<T1, T2, T3> source, Func<T1, T2, T3, TResult> evaluate)
    {
        return evaluate(source.Item1, source.Item2, source.Item3);
    }

    public static void Pipe<TSource>(this TSource source, Action<TSource> evaluate)
    {
        evaluate(source);
    }

    public static TResult Pipe<TSource, TResult>(this TSource source, Func<TSource, TResult> evaluate)
    {
        return evaluate(source);
    }

    public static TResult Pipe<TSource, TResult>(this TSource source, bool condition, Func<TSource, TResult> evaluate)
            where TSource : TResult
    {
        return condition ? evaluate(source) : source;
    }

    public static TSource PipeMaybe<TSource, TValue>(this TSource source, TValue? value, Func<TSource, TValue, TSource> evaluate)
        where TValue : class
    {
        return value == null ? source : evaluate(source, value);
    }

    public static TSource PipeMaybe<TSource, TValue>(this TSource source, TValue? value, Func<TSource, TValue, TSource> evaluate)
        where TValue: struct
    {
        return value == null ? source : evaluate(source, value.Value);
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult StaticCast<TSource, TResult>(this TSource source)
            where TSource : TResult
    {
        return source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult? AsCast<TResult>(this object source)
            where TResult : class
    {
        return source as TResult;
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<Exception> getNothingException)
            where TSource : class
    {
        return source ?? throw getNothingException();
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<Exception> getNothingException)
            where TSource : struct
    {
        return source ?? throw getNothingException();
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<string> getNothingExceptionMessage)
            where TSource : class
    {
        return source.FromMaybe(() => new Exception(getNothingExceptionMessage()));
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<string> getNothingExceptionMessage)
            where TSource : struct
    {
        return source.FromMaybe(() => new Exception(getNothingExceptionMessage()));
    }

    public static TSource FromMaybe<TSource>(this TSource source, string nothingExceptionMessage)
            where TSource : class
    {
        return source.FromMaybe(() => nothingExceptionMessage);
    }

    public static TSource? FromMaybe<TSource>(this TSource? source, bool condition, Func<Exception> getNothingException)
            where TSource : class
    {
        return condition && null == source ? throw getNothingException() : source;
    }

    public static TSource? FromMaybe<TSource>(this TSource? source, bool condition, Func<string> getNothingExceptionMessage)
            where TSource : class
    {
        return source.FromMaybe(condition, () => new Exception(getNothingExceptionMessage()));
    }

    public static TSource? FromMaybe<TSource>(this TSource? source, bool condition, string nothingExceptionMessage)
            where TSource : class
    {
        return source.FromMaybe(condition, () => nothingExceptionMessage);
    }

    public static TResult? MaybeString<TResult>(this string? source, Func<string, TResult> evaluate)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return default;
        }

        return evaluate(source);
    }

    [Obsolete("v10 This method will be protected in future")]
    public static void MaybeString(this string source, Action<string> evaluate)
    {
        if (!string.IsNullOrEmpty(source))
        {
            evaluate(source);
        }
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TSource Iterate<TSource>(this TSource source, Func<TSource, bool> condition, Func<TSource, TSource> iterateFunc)
    {
        while (condition(source))
        {
            source = iterateFunc(source);
        }

        return source;
    }

    public static TResult IfDefault<TResult>(this TResult source, TResult otherResult)
    {
        return source.IsDefault() ? otherResult : source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static string IfDefaultString(this string source, string otherResult)
    {
        return string.IsNullOrWhiteSpace(source) ? otherResult : source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static T Min<T>(this T source, T other)
            where T : IComparable<T>
    {
        return source.CompareTo(other) < 0 ? source : other;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static T Max<T>(this T source, T other)
            where T : IComparable<T>
    {
        return source.CompareTo(other) > 0 ? source : other;
    }

    public static bool IsDefault<T>(this T value)
    {
        return EqualityComparer<T>.Default.Equals(value, default(T));
    }

    [Obsolete("v10 This method will be protected in future")]
    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this TKey key, TValue value)
    {
        return new KeyValuePair<TKey, TValue>(key, value);
    }
}
