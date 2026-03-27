using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.TryResult.Base;

namespace Framework.Configuration.BLL;

public partial interface IDomainObjectModificationBLL
{
    /// <summary>
    /// Обработка модификаций хранимых в базе
    /// </summary>
    /// <param name="limit">Ограничение на количество обработанных модификаций</param>
    /// <returns>Количество обработанных модификаций</returns>
    ITryResult<int> Process(int limit = 1000);

    /// <summary>
    /// Получение состояния обработки очереди
    /// </summary>
    /// <returns></returns>
    QueueProcessingState GetProcessingState();
}
