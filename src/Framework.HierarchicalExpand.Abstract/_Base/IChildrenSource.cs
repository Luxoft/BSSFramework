namespace Framework.Persistent;

public interface IChildrenSource<out T>
{
    IEnumerable<T> Children
    {
        get;
    }
}
