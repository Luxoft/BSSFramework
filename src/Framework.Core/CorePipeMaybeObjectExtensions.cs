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
        if (fromJustAction == null) throw new ArgumentNullException(nameof(fromJustAction));

        if (maybeValue.HasValue)
        {
            fromJustAction(maybeValue.Value);
        }
        else
        {
            fromNothingAction?.Invoke();
        }
    }
}
