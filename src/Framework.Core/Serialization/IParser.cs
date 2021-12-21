namespace Framework.Core.Serialization
{
    public interface IParser<in TInput, out TResult>
    {
        TResult Parse(TInput input);
    }
}