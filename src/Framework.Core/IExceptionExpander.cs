namespace Framework.Core;

/// <summary>
/// Сервис для раскрытия шаблонных исключений
/// </summary>
public interface IExceptionExpander
{
    public const string ElementKey = "Element";

    /// <summary>
    /// Пыпытка раскрытие исключения
    /// </summary>
    /// <param name="exception">Базовое исклчюение</param>
    /// <returns></returns>
    Exception? TryExpand(Exception exception);

    /// <summary>
    /// Раскрытие исключения
    /// </summary>
    /// <param name="exception">Базовое исклчюение</param>
    /// <returns></returns>
    Exception Expand(Exception exception) => this.TryExpand(exception) ?? exception;
}
