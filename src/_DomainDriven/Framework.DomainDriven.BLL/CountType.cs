namespace Framework.Core;

/// <summary>
/// Флаг размерностей "1 или больше"
/// </summary>
[Flags]
public enum CountType
{
    /// <summary>
    /// Единица
    /// </summary>
    Single = 1,

    /// <summary>
    /// Коллеция
    /// </summary>
    Many = 2,

    /// <summary>
    /// Всё вместе
    /// </summary>
    Both = Single + Many
}
