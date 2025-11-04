namespace Framework.Persistent;

/// <summary>
/// Описывает деталь в связке мастер-деталь
/// </summary>
/// <typeparam name="TMaster">Тип мастера</typeparam>
public interface IDetail<out TMaster>
        where TMaster : class
{
    /// <summary>
    /// Ссылка на мастера
    /// </summary>
    TMaster? Master { get; }
}
