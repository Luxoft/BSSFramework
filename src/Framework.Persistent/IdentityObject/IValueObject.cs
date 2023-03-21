namespace Framework.Persistent;

public interface IValueObject<out TValue>
{
    TValue Value { get; }
}
