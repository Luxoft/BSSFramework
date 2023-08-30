namespace Framework.DomainDriven;

/// <summary>
/// Потребитель DAL-евентов
/// </summary>
public interface IDALListener
{
    /// <summary>
    /// Обработка евента
    /// </summary>
    /// <param name="eventArgs">Даннные о DAL-изменениях</param>
    void Process(DALChangesEventArgs eventArgs);
}
