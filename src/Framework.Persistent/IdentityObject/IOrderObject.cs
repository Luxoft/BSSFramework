namespace Framework.Persistent;

public interface IOrderObject<out T>
{
    T OrderIndex { get; }
}
