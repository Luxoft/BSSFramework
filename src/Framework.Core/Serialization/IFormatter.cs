namespace Framework.Core.Serialization
{
    public interface IFormatter<in TValue, out TResult>
    {
        TResult Format(TValue value);
    }
}