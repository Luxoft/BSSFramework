namespace Framework.Persistent
{
    public interface INumberObject<out TNumber>
    {
        TNumber Number { get; }
    }
}