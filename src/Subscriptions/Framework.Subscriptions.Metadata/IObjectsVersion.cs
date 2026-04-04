namespace Framework.Subscriptions.Metadata;

public interface IObjectsVersion<out T>
{
    T? Previous { get; }

    T? Current { get; }
}
