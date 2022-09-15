namespace Framework.Core;

/// <summary>
/// For same lazy object with covariant interface
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILazyObject<out T>
{
    bool IsValueCreated { get; }

    T Value { get; }
}
