namespace Framework.Parsing;

public static class ParserCombinatorExtensions
{
    public static Parser<TInput, TValue> Or<TInput, TValue>(
            this Parser<TInput, TValue> parser1,
            Parser<TInput, TValue> parser2)
    {
        return input => parser1(input) ?? parser2(input);
    }

    public static Parser<TInput, TValue2> And<TInput, TValue1, TValue2>(
            this Parser<TInput, TValue1> parser1,
            Parser<TInput, TValue2> parser2)
    {
        return input => parser2(parser1(input).Rest);
    }

    public static Parser<TInput, TValue> Or<TInput, TValue>(
            this Parser<TInput, TValue> parser1,
            Func<Parser<TInput, TValue>> parser2)
    {
        return input => parser1(input) ?? parser2()(input);
    }

    public static Parser<TInput, TValue2> And<TInput, TValue1, TValue2>(
            this Parser<TInput, TValue1> parser1,
            Func<Parser<TInput, TValue2>> parser2)
    {
        return input => parser2()(parser1(input).Rest);
    }





    public static Parser<TInput, Func<TArg, TResultValue>> Compose<TInput, TArg, TValue, TResultValue>(this Parser<TInput, Func<TArg, TValue>> p1, Parser<TInput, Func<TValue, TResultValue>> p2)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        return from f1 in p1
               from f2 in p2
               select new Func<TArg, TResultValue>(v => f2(f1(v)));
    }


    public static Parser<TInput, TValue> Pipe<TInput, TArg, TValue>(this Parser<TInput, TArg> p1, Parser<TInput, Func<TArg, TValue>> p2)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        return from v in p1
               from f in p2
               select f(v);
    }

    public static Parser<TInput, object> Box<TInput, TValue>(this Parser<TInput, TValue> parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        return from v in parser
               select (object)v;
    }


    public static ParserTableRow<TInput, TValue> ToRow<TInput, TValue>(this Parser<TInput, TValue> parser, Func<TValue> getDefaultValue)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));
        if (getDefaultValue == null) throw new ArgumentNullException(nameof(getDefaultValue));

        return new ParserTableRow<TInput, TValue>(parser, getDefaultValue);
    }
}
