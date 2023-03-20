namespace Framework.Persistent;

public interface ILocationObject<out TLocation>
{
    TLocation Location { get; }
}
