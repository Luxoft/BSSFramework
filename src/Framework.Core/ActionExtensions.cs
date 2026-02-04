using System.Threading.Tasks;
using CommonFramework;

namespace Framework.Core;

public static class ActionExtensions
{
    public static Action<T1, T2> Composite<T1, T2>(this IEnumerable<Action<T1, T2>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var cachedSource = source.ToArray();

        return (v1, v2) => cachedSource.Foreach(action => action(v1, v2));
    }

    public static Action<T1, T2> Composite<TSource, T1, T2>(this IEnumerable<TSource> source, Func<TSource, Action<T1, T2>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).Composite();
    }

    public static Func<T, object?> ToDefaultFunc<T>(this Action<T> action)
    {
        return a => { action(a); return null; };
    }

    public static Func<T1, T2, object?> ToDefaultFunc<T1, T2>(this Action<T1, T2> action)
    {
        return (a1, a2) => { action(a1, a2); return null; };
    }
}
