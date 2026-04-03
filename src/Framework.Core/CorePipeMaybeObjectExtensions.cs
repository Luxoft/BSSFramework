namespace Framework.Core;

public static class CorePipeMaybeObjectExtensions
{
    public static TResult IfDefault<TResult>(this TResult source, TResult otherResult) => source.IsDefault() ? otherResult : source;
}
