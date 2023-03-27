namespace Framework.DomainDriven;

/// <summary>
/// Параметры управления генерацией по модели
/// </summary>
[Flags]
public enum DirectMode
{
    /// <summary>
    /// Модель отдаётся в качестве резльтата метода
    /// </summary>
    Out = 1,

    /// <summary>
    /// Модель принимается в качестве параметра метода
    /// </summary>
    In = 2,
}
