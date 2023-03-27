namespace Framework.Exceptions;

/// <summary>
/// Сервис для раскрытия шаблонных исключений
/// </summary>
public interface IExceptionProcessor
{
    /// <summary>
    /// Раскрытие исключения
    /// </summary>
    /// <param name="exception">Базовое исклчюение</param>
    /// <returns></returns>
    Exception Process(Exception exception);
}
