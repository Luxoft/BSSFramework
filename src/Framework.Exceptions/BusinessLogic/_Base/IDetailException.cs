namespace Framework.Exceptions
{
    public interface IDetailException<out TDetail>
    {
        TDetail Detail { get; }
    }
}