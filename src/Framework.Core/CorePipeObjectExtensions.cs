namespace Framework.Core;

public static class CorePipeObjectExtensions
{
    public static TResult Pipe<T1, T2, TResult>(this Tuple<T1, T2> source, Func<T1, T2, TResult> evaluate)
    {
        return evaluate(source.Item1, source.Item2);
    }

    public static TResult? AsCast<TResult>(this object source)
        where TResult : class
    {
        return source as TResult;
    }

    public static T Min<T>(this T source, T other)
        where T : IComparable<T>
    {
        return source.CompareTo(other) < 0 ? source : other;
    }

    public static T Max<T>(this T source, T other)
        where T : IComparable<T>
    {
        return source.CompareTo(other) > 0 ? source : other;
    }

    public static TSource Self<TSource>(this TSource source, Action<TSource> evaluate)
    {
        evaluate(source);

        return source;
    }
}
