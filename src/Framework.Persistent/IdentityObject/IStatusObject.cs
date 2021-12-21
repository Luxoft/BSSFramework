namespace Framework.Persistent
{
    public interface IStatusObject<out TStatus>
    {
        TStatus Status { get; }
    }
}