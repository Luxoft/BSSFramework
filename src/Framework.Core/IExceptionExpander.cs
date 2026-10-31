namespace Framework.Core;

/// <summary>
/// Service for unwrapping template exceptions
/// </summary>
public interface IExceptionExpander
{
    public const string ElementKey = "Element";

    /// <summary>
    /// Attempt to unwrap the exception
    /// </summary>
    /// <param name="exception">Base exception</param>
    /// <returns></returns>
    Exception? TryExpand(Exception exception);

    /// <summary>
    /// Exception unwrapping
    /// </summary>
    /// <param name="exception">Base exception</param>
    /// <returns></returns>
    Exception Expand(Exception exception) => this.TryExpand(exception) ?? exception;
}
