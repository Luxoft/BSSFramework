using CommonFramework.Maybe;

namespace Framework.Core;

public static class CorePipeMaybeObjectExtensions
{
    public static TResult IfDefault<TResult>(this TResult source, TResult otherResult)
    {
        return source.IsDefault() ? otherResult : source;
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
}
