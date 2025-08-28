using CommonFramework;

namespace Framework.Core.Serialization;

public static class ParserExtensions
{
    public static IParser<TValue, TResult> WithCache<TValue, TResult>(this IParser<TValue, TResult> parser, IEqualityComparer<TValue> equalityComparer = null)
        where TValue : notnull
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        return new Parser<TValue, TResult>(FuncHelper.Create((TValue value) => parser.Parse(value)).WithCache(equalityComparer));
    }

    public static IParser<TValue, TResult> WithLock<TValue, TResult>(this IParser<TValue, TResult> parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        return new Parser<TValue, TResult>(FuncHelper.Create((TValue value) => parser.Parse(value)).WithLock());
    }
}
