namespace Framework.Core;

/// <summary>
/// Interface for converting an object to another type
/// </summary>
/// <typeparam name="TOut">Type the object is converted to</typeparam>
public interface IConvertible<out TOut>
{
    /// <summary>
    /// Conversion method
    /// </summary>
    /// <returns></returns>
    TOut Convert();
}
