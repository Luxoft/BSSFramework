namespace Framework.Core;

/// <summary>
/// Интерфейс конвертации объекта в другой тип
/// </summary>
/// <typeparam name="TOut">Тип в который конверитуерся объект</typeparam>
public interface IConvertible<out TOut>
{
    /// <summary>
    /// Метод конвертации
    /// </summary>
    /// <returns></returns>
    TOut Convert();
}
