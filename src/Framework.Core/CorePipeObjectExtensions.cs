#nullable enable

namespace Framework.Core;

public static class CorePipeObjectExtensions
{
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
