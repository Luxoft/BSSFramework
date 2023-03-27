using Framework.Core;

namespace Framework.Parsing;

public class ParserTableRow<TInput, TValue>
{
    public readonly Parser<TInput, TValue> Parser;
    public readonly Func<TValue> GetDefaultValue;

    public ParserTableRow(Parser<TInput, TValue> parser, Func<TValue> getDefaultValue)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));
        if (getDefaultValue == null) throw new ArgumentNullException(nameof(getDefaultValue));

        this.Parser = parser;
        this.GetDefaultValue = getDefaultValue;
    }
}

public abstract class Parsers<TInput>
{
    protected Parser<TInput, TResult> OfTable<T1, T2, T3, T4, T5, T6, TSepearator, TResult> (
            ParserTableRow<TInput, T1> p1,
            ParserTableRow<TInput, T2> p2,
            ParserTableRow<TInput, T3> p3,
            ParserTableRow<TInput, T4> p4,
            ParserTableRow<TInput, T5> p5,
            ParserTableRow<TInput, T6> p6,
            Parser<TInput, TSepearator> separator,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
    {
        var table = new Dictionary<string, Parser<TInput, object>>
                    {
                            { "p1", p1.Parser.Box() },
                            { "p2", p2.Parser.Box() },
                            { "p3", p3.Parser.Box() },
                            { "p4", p4.Parser.Box() },
                            { "p5", p5.Parser.Box() },
                            { "p6", p6.Parser.Box() },
                    };


        return from proccessResult in this.SubOfTable(table, separator)

               let v1 = (T1)proccessResult.GetValueOrDefault("p1", () => p1.GetDefaultValue())

               let v2 = (T2)proccessResult.GetValueOrDefault("p2", () => p2.GetDefaultValue())

               let v3 = (T3)proccessResult.GetValueOrDefault("p3", () => p3.GetDefaultValue())

               let v4 = (T4)proccessResult.GetValueOrDefault("p4", () => p4.GetDefaultValue())

               let v5 = (T5)proccessResult.GetValueOrDefault("p5", () => p5.GetDefaultValue())

               let v6 = (T6)proccessResult.GetValueOrDefault("p6", () => p6.GetDefaultValue())

               select resultSelector(v1, v2, v3, v4, v5, v6);
    }


    private Parser<TInput, Dictionary<string, object>> SubOfTable<TSepearator>(Dictionary<string, Parser<TInput, object>> table, Parser<TInput, TSepearator> separator)
    {
        var successRowParser = this.OneOfMany(table.Select(pair => pair.Value.Select(value => new KeyValuePair<string, object>(pair.Key, value))));

        return (from rowPair in successRowParser

                from subParseResult in (from sep in separator
                                        from subPairs in this.SubOfTable(table.Where(pair => pair.Key != rowPair.Key).ToDictionary(), separator)
                                        select subPairs).Or(() => this.Return(new Dictionary<string, object>()))

                select new[] { rowPair }.ToDictionary().Concat(subParseResult)).Or(() => this.Return(new Dictionary<string, object>()));
    }

    protected Parser<TInput, TValue> CatchParser<TValue>(Func<TValue> createFunc)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        try   { return this.Return(createFunc()); }
        catch { return this.Fault<TValue>(); }
    }


    public Parser<TInput, Ignore> Fault()
    {
        return this.Fault<Ignore>();
    }

    public Parser<TInput, TValue> Fault<TValue>()
    {
        return input => null;
    }

    public Parser<TInput, Ignore> Return()
    {
        return Parser<TInput>.Return(Ignore.Value);
    }

    public Parser<TInput, Func<TIdentity, TIdentity>> GetIdentity<TIdentity>()
    {
        return this.Return(new Func<TIdentity, TIdentity>(v => v));
    }

    public Parser<TInput, TValue> Return<TValue>(TValue value)
    {
        return input => new Result<TInput, TValue>(value, input);
    }

    public Parser<TInput, TValue> OneOfMany<TValue>(params Parser<TInput, TValue>[] parsers)
    {
        if (parsers == null) throw new ArgumentNullException(nameof(parsers));

        return this.OneOfMany((IEnumerable<Parser<TInput, TValue>>)parsers);
    }

    public Parser<TInput, TValue> OneOfMany<TValue>(IEnumerable<Parser<TInput, TValue>> parsers)
    {
        if (parsers == null) throw new ArgumentNullException(nameof(parsers));

        return parsers.Select(p => this.GetLazy(() => p)).Aggregate((p1, p2) => p1.Or(p2));
    }


    public Parser<TInput, TValue[]> Many<TValue>(Parser<TInput, TValue> parser)
    {
        return this.Many1(parser).Or(this.Return(new TValue[0]));
    }

    public Parser<TInput, TValue[]> Many1<TValue>(Parser<TInput, TValue> parser)
    {
        // fix stackoverflow
        return input =>
               {
                   var prevResult = parser(input);

                   if (prevResult == null)
                   {
                       return null;
                   }
                   else
                   {
                       for (var list = new List<TValue> { prevResult.Value };; list.Add(prevResult.Value))
                       {
                           var nextResult = parser(prevResult.Rest);

                           if (nextResult == null)
                           {
                               return new Result<TInput, TValue[]>(list.ToArray(), prevResult.Rest);
                           }
                           else
                           {
                               prevResult = nextResult;
                           }
                       }
                   }
               };

        //return from x in parser
        //       from xs in this.Many(parser)
        //       select x.Cons(xs);
    }

    public Parser<TInput, Ignore> TestYes<TValue>(Parser<TInput, TValue> testParser)
    {
        return input =>
               {
                   var prevResult = testParser(input);

                   if (prevResult == null)
                   {
                       return null;
                   }
                   else
                   {
                       return new Result<TInput, Ignore>(Ignore.Value, input);
                   }
               };
    }

    public Parser<TInput, Ignore> TestNo<TValue>(Parser<TInput, TValue> testParser)
    {
        return input =>
               {
                   var prevResult = testParser(input);

                   if (prevResult == null)
                   {
                       return new Result<TInput, Ignore>(Ignore.Value, input);
                   }
                   else
                   {
                       return null;
                   }
               };
    }


    public Parser<TInput, TValue[]> SepBy1<TValue, TSeparator>(Parser<TInput, TValue> parser, Parser<TInput, TSeparator> separatorParser)
    {
        return from x in parser

               from xs in this.Many(this.Pre(parser, separatorParser))

               select x.Cons(xs).ToArray();
    }

    public Parser<TInput, TValue[]> SepBy<TValue, TSeparator>(Parser<TInput, TValue> parser, Parser<TInput, TSeparator> separatorParser)
    {
        return this.SepBy1(parser, separatorParser).Or(this.Return(new TValue[0]));
    }

    public Parser<TInput, TValue> Pre<TValue, TOpen>(Parser<TInput, TValue> parser, Parser<TInput, TOpen> openParser)
    {
        return from _ in openParser
               from res in parser
               select res;
    }

    public Parser<TInput, TValue> Post<TValue, TClose>(Parser<TInput, TValue> parser, Parser<TInput, TClose> closeParser)
    {
        return from res in parser
               from __ in closeParser
               select res;
    }

    public Parser<TInput, TValue> Between<TValue, TOpen, TClose> (Parser<TInput, TValue> parser, Parser<TInput, TOpen> openParser, Parser<TInput, TClose> closeParser)
    {
        return this.Post(this.Pre (parser, openParser), closeParser);
    }


    public Parser<TInput, TValue> GetLazy<TValue>(Func<Parser<TInput, TValue>> getParser)
    {
        return input => getParser()(input);
    }
}

public static class Parser<TInput>
{
    public static Parser<TInput, TValue> Return<TValue>(TValue value)
    {
        return input => new Result<TInput, TValue>(value, input);
    }
}
