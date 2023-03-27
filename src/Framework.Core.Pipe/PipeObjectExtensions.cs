namespace Framework.Core;

public static class PipeObjectExtensions
{
    [Obsolete("v10 This method will be protected in future")]
    public static void Case<T>(this T source, T condition, Action action)
    {
        if (source.Equals(condition))
        {
            action();
        }
    }

    [Obsolete("v10 This method will be protected in future")]
    public static void Case<T>(this T source, Func<T, bool> condition, Action action)
    {
        if (condition(source))
        {
            action();
        }
    }

    public static TSource Self<TSource>(this TSource source, bool condition, Action<TSource> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        if (condition)
        {
            evaluate(source);
        }

        return source;
    }

    public static TSource Self<TSource>(this TSource source, Action<TSource> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        evaluate(source);

        return source;
    }

    public static void Pipe<T1, T2>(this Tuple<T1, T2> source, Action<T1, T2> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        evaluate(source.Item1, source.Item2);
    }

    public static TResult Pipe<T1, T2, TResult>(this Tuple<T1, T2> source, Func<T1, T2, TResult> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        return evaluate(source.Item1, source.Item2);
    }

    public static void Pipe<T1, T2, T3>(this Tuple<T1, T2, T3> source, Action<T1, T2, T3> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        evaluate(source.Item1, source.Item2, source.Item3);
    }

    public static TResult Pipe<T1, T2, T3, TResult>(this Tuple<T1, T2, T3> source, Func<T1, T2, T3, TResult> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        return evaluate(source.Item1, source.Item2, source.Item3);
    }

    public static void Pipe<TSource>(this TSource source, Action<TSource> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        evaluate(source);
    }

    public static TResult Pipe<TSource, TResult>(this TSource source, Func<TSource, TResult> evaluate)
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        return evaluate(source);
    }

    public static TResult Pipe<TSource, TResult>(this TSource source, bool condition, Func<TSource, TResult> evaluate)
            where TSource : TResult
    {
        if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

        return condition ? evaluate(source) : source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult StaticCast<TSource, TResult>(this TSource source)
            where TSource : TResult
    {
        return source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult AsCast<TResult>(this object source)
            where TResult : class
    {
        return source as TResult;
    }

    public static TSource FromMaybe<TSource>(this TSource source, Func<Exception> getNothingException)
            where TSource : class
    {
        if (getNothingException == null) throw new ArgumentNullException(nameof(getNothingException));

        if (null == source)
        {
            throw getNothingException();
        }

        return source;
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<Exception> getNothingException)
            where TSource : struct
    {
        if (getNothingException == null) throw new ArgumentNullException(nameof(getNothingException));

        if (null == source)
        {
            throw getNothingException();
        }

        return source.Value;
    }

    public static TSource FromMaybe<TSource>(this TSource source, Func<string> getNothingExceptionMessage)
            where TSource : class
    {
        if (getNothingExceptionMessage == null) throw new ArgumentNullException(nameof(getNothingExceptionMessage));

        return source.FromMaybe(() => new Exception(getNothingExceptionMessage()));
    }

    public static TSource FromMaybe<TSource>(this TSource? source, Func<string> getNothingExceptionMessage)
            where TSource : struct
    {
        if (getNothingExceptionMessage == null) throw new ArgumentNullException(nameof(getNothingExceptionMessage));

        return source.FromMaybe(() => new Exception(getNothingExceptionMessage()));
    }

    public static TSource FromMaybe<TSource>(this TSource source, string nothingExceptionMessage)
            where TSource : class
    {
        if (nothingExceptionMessage == null) throw new ArgumentNullException(nameof(nothingExceptionMessage));

        return source.FromMaybe(() => nothingExceptionMessage);
    }

    public static TSource FromMaybe<TSource>(this TSource source, bool condition, Func<Exception> getNothingException)
            where TSource : class
    {
        if (getNothingException == null) throw new ArgumentNullException(nameof(getNothingException));

        if (condition && null == source)
        {
            throw getNothingException();
        }

        return source;
    }

    public static TSource FromMaybe<TSource>(this TSource source, bool condition, Func<string> getNothingExceptionMessage)
            where TSource : class
    {
        if (getNothingExceptionMessage == null) throw new ArgumentNullException(nameof(getNothingExceptionMessage));

        return source.FromMaybe(condition, () => new Exception(getNothingExceptionMessage()));
    }

    public static TSource FromMaybe<TSource>(this TSource source, bool condition, string nothingExceptionMessage)
            where TSource : class
    {
        if (nothingExceptionMessage == null) throw new ArgumentNullException(nameof(nothingExceptionMessage));

        return source.FromMaybe(condition, () => nothingExceptionMessage);
    }

    [Obsolete("v10 This method will be protected in future")]
    public static string FromString(this string source, Func<Exception> getNothingException)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            throw getNothingException();
        }

        return source;
    }

    public static TResult MaybeString<TResult>(this string source, Func<string, TResult> evaluate)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return default(TResult);
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

    [Obsolete("Странный метод с WHERE, но при этом работает с одним объектом")]
    public static TSource MaybeWhere<TSource>(this TSource source, Func<TSource, bool> filter)
            where TSource : class
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        return source.Maybe(v => filter(v) ? source : null);
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TObject Do<TObject>(this TObject source, Action<TObject> action)
            where TObject : class
    {
        source.Maybe(action);
        return source;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult When<TResult>(this TResult source, Func<TResult, bool> condition, TResult elseResult)
    {
        return (condition(source)) ? source : elseResult;
    }

    [Obsolete("v10 This method will be protected in future")]
    public static TResult When<TSource, TResult>(this TSource source, Func<TSource, bool> condition, Func<TSource, TResult> selector, TResult elseResult)
    {
        return (condition(source)) ? selector(source) : elseResult;
    }

    [Obsolete("v10 Use ?.")]
    public static TResult IfNull<TResult>(this TResult source, TResult otherResult)
            where TResult : class
    {
        return source.IfDefault(otherResult);
    }

    public static TResult IfDefault<TResult>(this TResult source, TResult otherResult)
    {
        return source.IsDefault() ? otherResult : source;
    }

    [Obsolete("v10 Use ?.")]
    public static void IfNotNull<T>(this T? source, Action<T> action) where T : struct
    {
        if (source.HasValue)
        {
            action(source.Value);
        }
    }
    [Obsolete("v10 Use ?.")]
    public static void IfNotNull<T>(this T source, Action<T> action) where T : class
    {
        if (null != source)
        {
            action(source);
        }
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
    public static bool IsNullOrDefault<T>(T? value)
            where T : struct, IEquatable<T>
    {
        return value.GetValueOrDefault().Equals(default(T));
    }

    [Obsolete("v10 This method will be protected in future")]
    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this TKey key, TValue value)
    {
        return new KeyValuePair<TKey, TValue>(key, value);
    }
}
