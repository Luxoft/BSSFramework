namespace Framework.BLL.Domain.IdentityObject;

/// <summary>
/// Интерфейс для UpdateDTO
/// </summary>
public interface IUpdateDTO
{
    /// <summary>
    /// DTO является пустой (проверять нужно после сжатия)
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Сжатие полей
    /// </summary>
    void Compress();
}
