namespace Framework.Core.Serialization;

public class Parser<TValue, TResult> : IParser<TValue, TResult>
{
    private readonly Func<TValue, TResult> _parseFunc;


    public Parser(Func<TValue, TResult> parseFunc)
    {
        if (parseFunc == null) throw new ArgumentNullException(nameof(parseFunc));

        this._parseFunc = parseFunc;
    }


    public virtual TResult Parse(TValue value)
    {
        return this._parseFunc(value);
    }
}


//public class Parser<T> : Parser<string, T>
//{
//    public Parser(Func<string, T> parseFunc) : base(parseFunc)
//    {

//    }


//    public static readonly Parser<T> Default = new DefaultParser();


//    public class DefaultParser : Parser<T>
//    {
//        public DefaultParser() : base(ParserHelper.)
//        {

//        }


//        private
//    }
//}
