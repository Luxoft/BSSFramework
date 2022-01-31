#nullable enable
namespace Framework.Persistent;

public interface IParentSource<out T>
{
    T? Parent
    {
        get;
    }
}
