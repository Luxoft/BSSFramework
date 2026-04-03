namespace Framework.Core.Serialization;

public class Parser<TInput, TResult>(Func<TInput, TResult> parseFunc) : IParser<TInput, TResult>
{
    public virtual TResult Parse(TInput value) => parseFunc(value);
}
