using Framework.Application.Session.DALChanges;

namespace Framework.Application.DALListener;

/// <summary>
/// Потребитель DAL-евентов
/// </summary>
public interface IdalListener
{
    /// <summary>
    /// Обработка евента
    /// </summary>
    /// <param name="eventArgs">Даннные о DAL-изменениях</param>
    Task Process(DalChangesEventArgs eventArgs, CancellationToken cancellationToken);
}
