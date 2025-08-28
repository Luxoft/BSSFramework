#nullable enable

namespace Framework.Core;

public static class PipeObjectExtensions
{
    [Obsolete("v10 This method will be protected in future")]
    public static T Min<T>(this T source, T other)
            where T : IComparable<T>
    {
        throw new Exception("Use CommonFramework");
    }

    [Obsolete("v10 This method will be protected in future")]
    public static T Max<T>(this T source, T other)
            where T : IComparable<T>
    {
        throw new Exception("Use CommonFramework");
    }

    public static TSource Self<TSource>(this TSource source, Action<TSource> evaluate)
    {
        evaluate(source);

        return source;
    }
}
