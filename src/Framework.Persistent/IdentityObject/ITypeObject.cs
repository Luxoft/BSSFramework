namespace Framework.Persistent
{
    public interface ITypeObject<out TType>
    {
        TType Type { get; }
    }
}