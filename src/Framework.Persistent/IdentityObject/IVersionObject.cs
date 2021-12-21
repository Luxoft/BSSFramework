namespace Framework.Persistent
{
    public interface IVersionObject<out TVersion>
    {
        TVersion Version { get; }
    }
}