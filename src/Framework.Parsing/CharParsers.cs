using CommonFramework;

namespace Framework.Parsing;

public class CharParsers : CharParsers<string>
{
    public override Parser<string, char> AnyChar
    {
        get { return input => input.Any() ? new Result<string, char>(input.First(), input.Substring(1)) : null; }
    }


    public Parser<string, Ignore> Eof
    {
        get { return input => input.Any() ? null : new Result<string, Ignore>(Ignore.Value, input); }
    }

    public override Parser<string, string> TakeText(int charCount)
    {
        return input => input.Length < charCount ? null : new Result<string, string>(input.Substring(0, charCount), input.Substring(charCount));
    }
}


public abstract class CharParsers<TInput> : Parsers<TInput>
{
    public abstract Parser<TInput, char> AnyChar { get; }


    public Parser<TInput, string> Spaces
    {
        get
        {
            return from spaces in this.Many(this.Char(' '))

                   select new string(spaces);
        }
    }


    public Parser<TInput, TValue> BetweenBrackets<TValue>(Parser<TInput, TValue> parser)
    {
        return this.BetweenBrackets(parser, '(', ')');
    }

    public Parser<TInput, TValue> BetweenBrackets<TValue>(Parser<TInput, TValue> parser, char startBracket, char endBracket)
    {
        return this.Between(parser, this.PreSpaces(this.Char(startBracket)), this.PreSpaces(this.Char(endBracket)));
    }


    public Parser<TInput, TValue> BetweenSpaces<TValue>(Parser<TInput, TValue> parser)
    {
        return this.Between(parser, this.Spaces, this.Spaces);
    }

    public Parser<TInput, TValue> PreSpaces<TValue>(Parser<TInput, TValue> parser)
    {
        return this.Pre(parser, this.Spaces);
    }

    public Parser<TInput, TValue> PostSpaces<TValue>(Parser<TInput, TValue> parser)
    {
        return this.Post(parser, this.Spaces);
    }


    public Parser<TInput, string> Spaces1
    {
        get
        {
            return from spaces in this.Many1(this.Char(' '))

                   select new string(spaces);
        }
    }

    public Parser<TInput, TValue[]> SepBy<TValue>(Parser<TInput, TValue> parser, char separator)
    {
        return this.SepBy(parser, this.BetweenSpaces(this.Char(separator)));
    }

    public Parser<TInput, TValue[]> SepBy1<TValue>(Parser<TInput, TValue> parser, char separator)
    {
        return this.SepBy1(parser, this.BetweenSpaces(this.Char(separator)));
    }



    public Parser<TInput, char> Char(char ch)
    {
        return from c in this.AnyChar
               where c == ch
               select c;
    }

    public Parser<TInput, char> Char(params char[] chars)
    {
        return from c in this.AnyChar
               where chars.Contains(c)
               select c;
    }

    public Parser<TInput, bool> TryChar(char ch)
    {
        var c1 = from _ in this.Char(ch)
                 select true;

        var c2 = from _ in this.Return(ch)
                 select false;

        return c1.Or(c2);
    }

    public Parser<TInput, char> CharIgnoreCase(char ch)
    {
        return from c in this.AnyChar
               where char.ToLower(c) == char.ToLower(ch)
               select c;
    }



    public Parser<TInput, string> Variable
    {
        get
        {
            return from letter in this.Letter.Or(this.Char('_'))

                   from tail in this.Word

                   select letter + tail;
        }
    }

    public Parser<TInput, string> Word
    {
        get { return this.Many(this.VariableBody).Select(chars => new string(chars)); }
    }

    public Parser<TInput, string> Word1
    {
        get { return this.Many1(this.VariableBody).Select(chars => new string(chars)); }
    }

    public Parser<TInput, char> VariableBody
    {
        get { return this.LetterOrDigit.Or(this.Char('_')); }
    }

    public Parser<TInput, char> LetterOrDigit
    {
        get { return this.Char(char.IsLetterOrDigit); }
    }

    public Parser<TInput, char> Letter
    {
        get { return this.Char(char.IsLetter); }
    }

    public Parser<TInput, char> Digit
    {
        get { return this.Char(char.IsDigit); }
    }

    public Parser<TInput, string> Digits
    {
        get { return this.Many(this.Digit).Select(chars => new string(chars)); }
    }

    public Parser<TInput, string> Digits1
    {
        get { return this.Digits.Where(str => str.Any()); }
    }

    public Parser<TInput, char> Char(Func<char, bool> pred)
    {
        return this.AnyChar.Where(pred);
    }

    public Parser<TInput, string> String(string pattern)
    {
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));

        return from str in this.TakeText(pattern.Length)

               where str == pattern

               select str;
    }


    public Parser<TInput, string> StringIgnoreCase(string pattern)
    {
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));

        return from str in this.TakeText(pattern.Length)

               where str.Equals(pattern, StringComparison.CurrentCultureIgnoreCase)

               select str;
    }

    public virtual Parser<TInput, string> TakeTo(string endToken)
    {
        if (endToken == null) throw new ArgumentNullException(nameof(endToken));

        var successParser = from _ in this.StringIgnoreCase(endToken)
                            select "";

        var nextParser = from c in this.AnyChar

                         from next in this.TakeTo(endToken)
                         select c + next;

        return successParser.Or(nextParser);
    }



    public Parser<TInput, string> TakeInBracket(string startBracket, string endBracket)
    {
        if (startBracket == null) throw new ArgumentNullException(nameof(startBracket));
        if (endBracket == null) throw new ArgumentNullException(nameof(endBracket));

        return from _ in this.StringIgnoreCase(startBracket)

               from result in this.TakeTo(endBracket)

               select result;
    }


    public Parser<TInput, string> TakeWhile(Func<char, bool> pred)
    {
        return from v in this.Many(this.Char(pred))
               select new string(v);
    }


    public Parser<TInput, string> TakeWhile1(Func<char, bool> pred)
    {
        return from v in this.Many1(this.Char(pred))
               select new string(v);
    }

    public Parser<TInput, string> TakeLine()
    {
        return from v in this.TakeWhile(c => c != '\r' && c != '\n')

               from _ in this.TryEndLine()

               select v;
    }

    public Parser<TInput, bool> TryEndLine()
    {
        return from v1 in this.TryChar('\r')
               from v2 in this.TryChar('\n')

               select v1 || v2;
    }

    public virtual Parser<TInput, string> TakeText(int charCount)
    {
        return charCount == 0

                       ? this.Return("")
                       : from c in this.AnyChar

                         from tail in this.TakeText(charCount - 1)

                         select c + tail;
    }


    public Parser<TInput, Guid> GuidParser
    {
        get
        {
            var guidParser = from text in this.TakeText(36)

                             from result in this.CatchParser(() => Guid.Parse(text))

                             select result;


            var withBrackets = this.Between(guidParser, this.Char('{'), this.Char('}'));


            return withBrackets.Or(guidParser);
        }
    }


    protected Parser<TInput, TResult> GetSignDigitsParser<TResult>(Func<string, TResult> parseFunc)
    {
        if (parseFunc == null) throw new ArgumentNullException(nameof(parseFunc));

        return from isNegate in this.TryChar('-')
               from value in this.Digits1
               from result in this.CatchParser(() => parseFunc((isNegate ? "-" : string.Empty) + value))
               select result;
    }

    public Parser<TInput, short> Int16Parser
    {
        get { return this.GetSignDigitsParser(short.Parse); }
    }

    public Parser<TInput, int> Int32Parser
    {
        get { return this.GetSignDigitsParser(int.Parse); }
    }

    public Parser<TInput, long> Int64Parser
    {
        get { return this.GetSignDigitsParser(long.Parse); }
    }


    public Parser<TInput, TValue> FromDictionary<TKey, TValue, TParseKeyResult>(Dictionary<TKey, TValue> dictionary, Func<TKey, Parser<TInput, TParseKeyResult>> getKeyParser)
    {
        if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
        if (getKeyParser == null) throw new ArgumentNullException(nameof(getKeyParser));


        return dictionary.Aggregate(this.Fault<TValue>(), (state, pair) => state.Or(() => from _ in getKeyParser(pair.Key)
                                                                                        select pair.Value));

    }
}
